using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Infrastructure;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using FreedomUtils.LINQ;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Managers
{
    public enum CharGender
    {
        Male = 0,
        Female = 1
    }

    public enum CharClass
    {
        None = 0,
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        Monk = 10,
        Druid = 11,
        DemonHunter = 12
    }

    public enum CharRace
    {
        None = 0,
        Human = 1,
        Orc = 2,
        Dwarf = 3,
        NightElf = 4,
        Forsaken = 5,
        Tauren = 6,
        Gnome = 7,
        Troll = 8,
        Goblin = 9,
        BloodElf = 10,
        Draenei = 11,
        Worgen = 22,
        PandarenNeutral = 24,
        PandarenAlliance = 25,
        PandarenHorde = 26,
        Nightborne = 27,
        HighmountainTauren = 28,
        VoidElf = 29,
        LightforgedDraenei = 30
    }

    public class CharacterManager
    {
        public const int MaxCharacterSlotsConst = 11;

        public static List<Character> GetAccountCharacters(int gameAccId)
        {
            using (var db = new DbCharacters())
            {
                return db.Characters.Where(c => c.GameAccountId == gameAccId).ToList();
            }
        }

        public static List<Character> GetOnlineCharacters()
        {
            using (var db = new DbCharacters())
            {
                return db.Characters.Where(c => c.IsOnline).ToList();
            }
        }

        public static bool DoesCharacterBelongTo(int charId, int gameAccId)
        {
            using (var db = new DbCharacters())
            {
                return db.Characters.Where(c => c.Id == charId && c.GameAccountId == gameAccId).FirstOrDefault() != null;
            }
        }

        public static bool IsGameAccFull(int gameAccId)
        {
            return GetAccountCharacters(gameAccId)
                .Where(c => c.DeleteDate == null)
                .Count() >= MaxCharacterSlotsConst;
        }

        public static void TransferCharacter(int charId, int gameAccId)
        {
            using (var db = new DbCharacters())
            {
                var character = db.Characters.Find(charId);
                character.GameAccountId = gameAccId;
                db.Entry(character).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static bool IsGMOn(int charId)
        {
            using (var db = new DbCharacters())
            {
                var character = db.Characters.Find(charId);
                short extraFlags = character.ExtraFlags;
                int GMStatus = extraFlags & 1;
                if (GMStatus == 1)
                    return true;
                else
                    return false;
            }
        }

        public static List<Character> DTGetFilteredOnlineCharacters(
            ref int recordsTotal,
            ref int recordsFiltered,
            int start,
            int length,
            IEnumerable<DTColumn> columnList,
            IEnumerable<DTOrder> orderList,
            bool allowUsernameFilter = false)
        {
            var list = new List<Character>();

            using (var db = new DbCharacters())
            {
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
                list = db.Characters
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
                recordsTotal = db.Characters.Where(c => c.IsOnline).Count();
                list = list.Skip(start).Take(length).ToList();                               
            }

            return list;
        }
    }
}
