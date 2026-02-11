using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Characters
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
        LightforgedDraenei = 30,

    }

    [Table("characters")]
    public class Character : EntityBase
    {
        [Column("guid")]
        [Key]
        public override int Id { get; set; }

        [Column("account")]
        public int GameAccountId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("class")]
        public CharClass Class { get; set; }

        [Column("gender")]
        public CharGender Gender { get; set; }

        [Column("race")]
        public CharRace Race { get; set; }

        [Column("slot")]
        public int Slot { get; set; }

        [Column("map")]
        public short MapId { get; set; }

        [Column("zone")]
        public short ZoneId { get; set; }

        [Column("online")]
        public bool IsOnline { get; set; }

        [Column("extra_flags")]
        public short ExtraFlags { get; set; }

        [Column("deleteDate")]
        public DateTime? DeleteDate { get; set; }

        [Column("latency")]
        public int Latency { get; set; }

        [NotMapped]
        public CharData CharData { get; set; }

        public List<CharacterCustomization> Customizations { get; set; }

        public List<CharacterInventorySlot> InventorySlots { get; set; }

        public List<ItemInstance> Items { get; set; }
    }
}
