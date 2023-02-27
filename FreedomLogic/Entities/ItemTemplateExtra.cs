using FreedomLogic.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FreedomLogic.Entities
{


    [Table("item_template_extra")]
    public class ItemTemplateExtra
    {
        [Key]
        [Column("entry_id")]
        public int ItemId { get; set; }

        [Column("hidden")]
        public bool Hidden { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("inventory_type")]
        public InventoryType InventoryType { get; set; }

        [Column("class")]
        public ItemClass ItemClass { get; set; }

        [Column("subclass")]
        public byte ItemSubclass { get; set; }

        public virtual ItemBonusIdInfo BonusIdInfo { get; set; }
        public virtual NpcItemInfo NpcItemInfo { get; set; }
    }

    public enum InventoryType : byte
    {
        NonEquip = 0,
        Head = 1,
        Neck = 2,
        Shoulders = 3,
        Body = 4,
        Chest = 5,
        Waist = 6,
        Legs = 7,
        Feet = 8,
        Wrists = 9,
        Hands = 10,
        Finger = 11,
        Trinket = 12,
        Weapon = 13,
        Shield = 14,
        Ranged = 15,
        Cloak = 16,
        TwoHandedWeapon = 17,
        Bag = 18,
        Tabard = 19,
        Robe = 20,
        WeaponMainHand = 21,
        WeaponOffHand = 22,
        Holdable = 23,
        Ammo = 24,
        Thrown = 25,
        RangedRight = 26,
        Quiver = 27,
        Relic = 28
    }

    public enum ItemClass : byte
    {
        Consumable = 0,
        Container = 1,
        Weapon = 2,
        Gem = 3,
        Armor = 4,
        Reagent = 5,
        Projectile = 6,
        TradeGoods = 7,
        ItemEnhancement = 8,
        Recipe = 9,
        Money = 10, // OBSOLETE
        Quiver = 11,
        Quest = 12,
        Key = 13,
        Permanent = 14, // OBSOLETE
        Miscellaneous = 15,
        Glyph = 16,
        BattlePets = 17,
        WowToken = 18
    };

    public enum ItemSubclassConsumable : byte
    {
        Consumable = 0,
        Potion = 1,
        Elixir = 2,
        Flask = 3,
        Scroll = 4,
        FoodDrink = 5,
        ItemEnhancement = 6,
        Bandage = 7,
        ConsumableOther = 8,
        VantusRune = 9
    };

    public enum ItemSubclassContainer : byte
    {
        Container = 0,
        SoulContainer = 1,
        HerbContainer = 2,
        EnchantingContainer = 3,
        EngineeringContainer = 4,
        GemContainer = 5,
        MiningContainer = 6,
        LeatherworkingContainer = 7,
        InscriptionContainer = 8,
        TackleContainer = 9,
        CookingContainer = 10
    };

    public enum ItemSubclassWeapon : byte
    {
        Axe = 0,  // One-Handed Axes
        TwoHandedAxe = 1,  // Two-Handed Axes
        Bow = 2,
        Gun = 3,
        Mace = 4,  // One-Handed Maces
        TwoHandedMace = 5,  // Two-Handed Maces
        Polearm = 6,
        Sword = 7,  // One-Handed Swords
        TwoHandedSword = 8,  // Two-Handed Swords
        Warglaives = 9,
        Staff = 10,
        Exotic = 11, // One-Handed Exotics
        TwoHandedExotic = 12, // Two-Handed Exotics
        FistWeapon = 13,
        Miscellaneous = 14,
        Dagger = 15,
        Thrown = 16,
        Spear = 17,
        Crossbow = 18,
        Wand = 19,
        FishingPole = 20
    };

    public enum ItemSubclassGem : byte
    {
        Intellect = 0,
        Agility = 1,
        Strength = 2,
        Stamina = 3,
        Spirit = 4,
        CriticalStrike = 5,
        Mastery = 6,
        Haste = 7,
        Versatility = 8,
        Other = 9,
        MultipleStats = 10,
        ArtifactRelic = 11
    };

    public enum ItemSubclassArmor : byte
    {
        Miscellaneous = 0,
        Cloth = 1,
        Leather = 2,
        Mail = 3,
        Plate = 4,
        Cosmetic = 5,
        Shield = 6,
        Libram = 7,
        Idol = 8,
        Totem = 9,
        Sigil = 10,
        Relic = 11,
    };

    public enum ItemSubclassReagent : byte
    {
        Reagent = 0,
        Keystone = 1
    };

    public enum ItemSubclassProjectile : byte
    {
        Wand = 0, // OBSOLETE
        Bolt = 1, // OBSOLETE
        Arrow = 2,
        Bullet = 3,
        Thrown = 4  // OBSOLETE
    };

    public enum ItemSubclassTradeGoods : byte
    {
        TradeGoods = 0,
        Parts = 1,
        Explosives = 2,
        Devices = 3,
        Jewelcrafting = 4,
        Cloth = 5,
        Leather = 6,
        MetalStone = 7,
        Meat = 8,
        Herb = 9,
        Elemental = 10,
        TradeGoodsOther = 11,
        Enchanting = 12,
        Material = 13,
        Enchantment = 14,
        WeaponEnchantment = 15,
        Inscription = 16,
        ExplosivesDevices = 17
    };

    public enum ItemSubclassItemEnhancement : byte
    {
        Head = 0,
        Neck = 1,
        Shoulder = 2,
        Cloak = 3,
        Chest = 4,
        Wrist = 5,
        Hands = 6,
        Waist = 7,
        Legs = 8,
        Feet = 9,
        Finger = 10,
        Weapon = 11,
        TwoHandedWeapon = 12,
        ShieldOffHand = 13
    };

    public enum ItemSubclassRecipe : byte
    {
        Book = 0,
        LeatherworkingPattern = 1,
        TailoringPattern = 2,
        EngineeringSchematic = 3,
        Blacksmithing = 4,
        CookingRecipe = 5,
        AlchemyRecipe = 6,
        FirstAidManual = 7,
        EnchantingFormula = 8,
        FishingManual = 9,
        JewelcraftingRecipe = 10,
        InscriptionTechnique = 11
    };

    public enum ItemSubclassQuiver : byte
    {
        Quiver0 = 0, // OBSOLETE
        Quiver1 = 1, // OBSOLETE
        Quiver = 2,
        AmmoPouch = 3
    };

    public enum ItemSubclassQuest : byte
    {
        Quest = 0,
        QuestUnk3 = 3, // 1 item (33604)
        QuestUnk8 = 8, // 2 items (37445, 49700)
    };

    public enum ItemSubclassKey : byte
    {
        Key = 0,
        Lockpick = 1
    };

    public enum ItemSubclassPermanent : byte
    {
        Permanent = 0
    };

    public enum ItemSubclassJunk : byte
    {
        Junk = 0,
        Reagent = 1,
        CompanionPet = 2,
        Holiday = 3,
        Other = 4,
        Mount = 5,
    };

    public enum ItemSubclassGlyph : byte
    {
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
    };

    public enum ItemSubclassBattlePet : byte
    {
        BattlePet = 0
    };

    public enum ItemSubclassWowToken : byte
    {
        WowToken = 0
    };
}

public static class ItemTemplateExtraExtensions
{
    public static string ToItemSubclassName(this byte subclass, ItemClass itemClass)
    {
        switch (itemClass)
        {
            case ItemClass.Consumable: return ((ItemSubclassConsumable)subclass).ToString();
            case ItemClass.Container: return ((ItemSubclassContainer)subclass).ToString();
            case ItemClass.Weapon: return ((ItemSubclassWeapon)subclass).ToString();
            case ItemClass.Gem: return ((ItemSubclassGem)subclass).ToString();
            case ItemClass.Armor: return ((ItemSubclassArmor)subclass).ToString();
            case ItemClass.Reagent: return ((ItemSubclassReagent)subclass).ToString();
            case ItemClass.Projectile: return ((ItemSubclassProjectile)subclass).ToString();
            case ItemClass.TradeGoods: return ((ItemSubclassTradeGoods)subclass).ToString();
            case ItemClass.ItemEnhancement: return ((ItemSubclassItemEnhancement)subclass).ToString();
            case ItemClass.Recipe: return ((ItemSubclassRecipe)subclass).ToString();
            case ItemClass.Quiver: return ((ItemSubclassQuiver)subclass).ToString();
            case ItemClass.Quest: return ((ItemSubclassQuest)subclass).ToString();
            case ItemClass.Key: return ((ItemSubclassKey)subclass).ToString();
            case ItemClass.Permanent: return ((ItemSubclassPermanent)subclass).ToString();
            case ItemClass.Glyph: return ((ItemSubclassGlyph)subclass).ToString();
            case ItemClass.BattlePets: return ((ItemSubclassBattlePet)subclass).ToString();
            case ItemClass.WowToken: return ((ItemSubclassWowToken)subclass).ToString();
            default: return "Unknown";
        }
    }
}