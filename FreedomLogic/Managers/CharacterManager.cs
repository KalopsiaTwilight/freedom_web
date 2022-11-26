using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomUtils.DataTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomLogic.Managers
{
    public class CharacterManager
    {
        public const int MaxCharacterSlotsConst = 11;

        private readonly DbCharacters _charactersDb;

        public CharacterManager(DbCharacters charactersDb)
        {
            _charactersDb = charactersDb;
        }

        public List<Character> GetAccountCharacters(int gameAccId)
        {
            return _charactersDb.Characters.Where(c => c.GameAccountId == gameAccId).ToList();
        }

        public List<Character> GetOnlineCharacters()
        {
            return _charactersDb.Characters.Where(c => c.IsOnline).ToList();
        }

        public bool DoesCharacterBelongTo(int charId, int gameAccId)
        {
            return _charactersDb.Characters.Where(c => c.Id == charId && c.GameAccountId == gameAccId).FirstOrDefault() != null;
        }

        public bool IsGameAccFull(int gameAccId)
        {
            return GetAccountCharacters(gameAccId)
                .Where(c => c.DeleteDate == null)
                .Count() >= MaxCharacterSlotsConst;
        }

        public void TransferCharacter(int charId, int gameAccId)
        {
            var character = _charactersDb.Characters.Find(charId);
            character.GameAccountId = gameAccId;
            _charactersDb.Entry(character).State = EntityState.Modified;
            _charactersDb.SaveChanges();
        }

        public bool IsGMOn(int charId)
        {
            var character = _charactersDb.Characters.Find(charId);
            short extraFlags = character.ExtraFlags;
            int GMStatus = extraFlags & 1;
            if (GMStatus == 1)
                return true;
            else
                return false;
        }

        public List<Character> DTGetFilteredOnlineCharacters(
            ref int recordsTotal,
            ref int recordsFiltered,
            int start,
            int length,
            IEnumerable<DTColumn> columnList,
            IEnumerable<DTOrder> orderList,
            bool allowUsernameFilter = false)
        {
            var list = new List<Character>();

            // [1] - Name
            // [2] - Owner
            // [3] - Race
            // [4] - Class
            // [5] - Map
            // [6] - Zone
            var colArray = columnList.ToArray();
            var orderArray = orderList.ToArray();

            // Can't use array in LINQ directly, because: http://stackoverflow.com/a/8354049
            var filterName = colArray[1].Search.Value;
            var filterOwner = colArray[2].Search.Value;
            var filterRace = colArray[3].Search.Value;
            var filterClass = colArray[4].Search.Value;
            var filterMap = colArray[5].Search.Value;
            var filterZone = colArray[6].Search.Value;

            // Set up basic filter query parts
            list = _charactersDb.Characters
                .Where(c => c.IsOnline)
                .Where(c => c.Name.ToUpper().Contains(filterName.ToUpper()))
                .ToList() // further c.CharData filtering is not supported by queries
                .Where(c => c.CharData.WebUser.DisplayName.ToUpper().Contains(filterOwner.ToUpper()) || (c.CharData.BnetAccount.UsernameEmail.ToUpper().Contains(filterOwner.ToUpper()) && allowUsernameFilter))
                .Where(c => c.CharData.RaceData.Name.ToUpper().Contains(filterRace.ToUpper()))
                .Where(c => c.CharData.ClassData.Name.ToUpper().Contains(filterClass.ToUpper()))
                .Where(c => c.CharData.MapName.ToUpper().Contains(filterMap.ToUpper()))
                .Where(c => c.CharData.ZoneName.ToUpper().Contains(filterZone.ToUpper()))
                .ToList();

            // Column ordering
            list.Sort(delegate (Character c1, Character c2)
            {
                foreach (var order in orderArray)
                {
                    string colName = colArray[order.Column].Data;

                    var property = c1.GetType().GetProperty(colName);

                    if (property == null)
                        continue;

                    IComparable c1value = property.GetValue(c1) as IComparable;
                    IComparable c2value = property.GetValue(c2) as IComparable;

                    if (c1value == null || c2value == null)
                        continue;

                    int result = c1value.CompareTo(c2value);

                    if (order.Dir == "desc")
                        result = result * (-1);

                    if (result != 0)
                        return result;
                }

                // Sort by primary key as last resort
                return c1.Id.CompareTo(c2.Id);
            });

            // Load and set results
            recordsFiltered = list.Count;
            recordsTotal = _charactersDb.Characters.Where(c => c.IsOnline).Count();
            list = list.Skip(start).Take(length).ToList();

            return list;
        }
    }
}
