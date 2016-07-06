using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    public class CharData
    {
        public CharData()
        {
        }

        public bool Load(int charId)
        {
            var character = DbManager.GetByKey<Character, DbCharacters>(charId);
            GameAccount = DbManager.GetByKey<GameAccount, DbAuth>(character.GameAccountId);            

            if (GameAccount == null || character == null)
                return false;

            BnetAccount = DbManager.GetByKey<BnetAccount, DbAuth>(GameAccount.BnetAccountId);

            if (BnetAccount == null)
                return false;

            using (var dbFreedom = new DbFreedom())
            {
                WebUser = dbFreedom.Users.Where(u => u.BnetAccountId == BnetAccount.Id).FirstOrDefault();
            }

            if (WebUser == null)
                return false;

            GameAccountAccess = AccountManager.GetGameAccAccess(GameAccount.Id);
            GameAccountCharacters = CharacterManager.GetAccountCharacters(GameAccount.Id);
            ClassData = DbManager.GetByKey<ClassInfo, DbFreedom>((int)character.Class);
            RaceData = DbManager.GetByKey<RaceInfo, DbFreedom>((int)character.Race);

            if (ClassData == null || RaceData == null)
                return false;

            var mapInfo = DbManager.GetByKey<MapInfo, DbFreedom>(character.MapId);
            if (mapInfo == null)
                MapName = "Unknown";
            else
                MapName = mapInfo.Name;

            var zoneInfo = DbManager.GetByKey<ZoneInfo, DbFreedom>(character.ZoneId);
            if (zoneInfo == null)
                ZoneName = "Unknown";
            else
                ZoneName = zoneInfo.Name;

            return true;
        }

        public string ZoneName { get; set; }

        public string MapName { get; set; }

        public RaceInfo RaceData { get; set; }

        public ClassInfo ClassData { get; set; }

        public User WebUser { get; set; }

        public List<Character> GameAccountCharacters { get; set; }

        public BnetAccount BnetAccount { get; set; }

        public GameAccount GameAccount { get; set; }

        public GameAccountAccess GameAccountAccess { get; set; }
    }
}
