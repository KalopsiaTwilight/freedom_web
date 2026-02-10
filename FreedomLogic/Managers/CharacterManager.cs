using FreedomLogic.DAL;
using FreedomLogic.Entities.Characters;
using FreedomLogic.Services;
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
        private readonly ExtraDataLoader _extraLoader;

        public CharacterManager(DbCharacters charactersDb, ExtraDataLoader extraLoader)
        {
            _charactersDb = charactersDb;
            _extraLoader = extraLoader; 
        }

        public Character GetCharacterById(int id)
        {
            return _charactersDb.Characters.FirstOrDefault(c => c.Id == id);
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
            bool allowUsernameFilter = false,
            string search = "")
        {
            var list = new List<Character>();

            var colArray = columnList.ToArray();
            var orderArray = orderList.ToArray();


            // Set up basic filter query parts
            list = _charactersDb.Characters
                .Where(c => c.IsOnline)
                .Where(c => c.Name.ToUpper().Contains(search.ToUpper()))
                .ToList();
            // Load extra filtering.
            list.ForEach(c => _extraLoader.LoadExtraCharData(c));
            list = list
                .Where(c => c.CharData.WebUser.DisplayName.ToUpper().Contains(search.ToUpper())
                         || (c.CharData.BnetAccount.UsernameEmail.ToUpper().Contains(search.ToUpper()) && allowUsernameFilter)
                         || c.CharData.RaceData.Name.ToUpper().Contains(search.ToUpper())
                         || c.CharData.ClassData.Name.ToUpper().Contains(search.ToUpper())
                         || c.CharData.MapName.ToUpper().Contains(search.ToUpper())
                         || c.CharData.ZoneName.ToUpper().Contains(search.ToUpper())
                )
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
