using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Services
{

    public class ExtraDataLoader
    {
        private readonly DbFreedom _freedomDb;
        private readonly DbAuth _authDb;
        private readonly DbCharacters _charactersDb;

        public ExtraDataLoader(DbFreedom freedomDb, DbAuth authDb, DbCharacters charactersDb)
        {
            _freedomDb = freedomDb;
            _authDb = authDb;
            _charactersDb = charactersDb;
        }

        public bool LoadExtraUserData(User user)
        {
            var userData = new UserData
            {
                WebUser = _freedomDb.Users.Find(user.Id)
            };
            if (userData.WebUser == null)
                return false;

            userData.BnetAccount = _authDb.BnetAccounts.Find(userData.WebUser.BnetAccountId);
            if (userData.BnetAccount == null)
                return false;

            userData.GameAccount = _authDb.GameAccounts
                    .AsNoTracking()
                    .Where(a => a.BnetAccountId == userData.BnetAccount.Id && a.BnetAccountIndex == 1)
                    .FirstOrDefault();
            if (userData.GameAccount == null)
                return false;


            userData.GameAccountAccess = GetGameAccountAccess(userData.GameAccount.Id);
            userData.GameAccountCharacters = _charactersDb.Characters
                .Where(c => c.GameAccountId == userData.GameAccount.Id)
                .ToList();
            user.UserData = userData;
            return true;
        }

        public bool LoadExtraCharData(Character character)
        {
            var charData = new CharData();
            charData.GameAccount = _authDb.GameAccounts.Find(character.GameAccountId);
            if (charData.GameAccount == null || character == null)
                return false;

            charData.BnetAccount = _authDb.BnetAccounts.Find(charData.GameAccount.Id);
            if (charData.BnetAccount == null)
                return false;

            charData.WebUser = _freedomDb.Users
                .Where(u => u.BnetAccountId == charData.BnetAccount.Id)
                .FirstOrDefault();
            if (charData.WebUser == null)
                return false;

            charData.GameAccountAccess = GetGameAccountAccess(charData.GameAccount.Id);
            charData.GameAccountCharacters = _charactersDb.Characters
                .Where(c => c.GameAccountId == charData.GameAccount.Id)
                .ToList();
            charData.ClassData = _freedomDb.ClassInfos.Find((int)character.Class);
            charData.RaceData = _freedomDb.RaceInfos.Find((int)character.Race);

            if (charData.ClassData == null || charData.RaceData == null)
                return false;

            var mapInfo = _freedomDb.MapInfos.Find(character.MapId);
            if (mapInfo == null)
                charData.MapName = "Unknown";
            else
                charData.MapName = mapInfo.Name;

            var zoneInfo = _freedomDb.ZoneInfos.Find(character.ZoneId);
            if (zoneInfo == null)
                charData.ZoneName = "Unknown";
            else
                charData.ZoneName = zoneInfo.Name;

            charData.ExtraFlags = _charactersDb.Characters.Find(character.ExtraFlags);

            return true;
        }

        private GameAccountAccess GetGameAccountAccess(int gameAccId)
        {
            GameAccountAccess gameAccAccess = _authDb.GameAccountAccesses.Find(gameAccId);
            if (gameAccAccess == null)
            {
                gameAccAccess = new GameAccountAccess()
                {
                    Id = gameAccId,
                    GMLevel = GMLevel.Player
                };

                _authDb.GameAccountAccesses.Add(gameAccAccess);
                _authDb.SaveChanges();
            }
            return gameAccAccess;
        }
    }
}
