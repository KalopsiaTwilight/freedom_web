using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Infrastructure;
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
        Druid = 11
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
        PandarenHorde = 26
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
    }
}
