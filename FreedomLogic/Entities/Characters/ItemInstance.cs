using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Characters
{

    [Table("item_instance")]
    public class ItemInstance
    {
        [Column("guid")]
        [Key]
        public int Id { get; set; }
        [Column("itemEntry")]
        public int ItemId { get; set; }
        [Column("owner_guid")]
        public int OwnerId { get; set; }
        [Column("creatorGuid")]
        public int Creator { get; set; }
        [Column("giftCreatorGuid")]
        public int GiftCreator { get; set; }
        [Column("count")]
        public int Count { get; set; }
        [Column("duration")]
        public int Duration { get; set; }
        [Column("charges")]
        public string Charges { get; set; }
        [Column("flags")]
        public int Flags { get; set; }
        [Column("enchantments")]
        public string Enchantments { get; set; }
        [Column("randomBonusListId")]
        public int RandomBonusListId { get; set; }
        [Column("durability")]
        public int Durability { get; set; }
        [Column("playedTime")]
        public int PlayedTime { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("transmogrification")]
        public int TransmogrificationId { get; set; }
        [Column("enchantIllusion")]
        public int EnchantIllusion { get; set; }
        [Column("battlePetSpeciesId")]
        public int BattlePetSpeciesId { get; set; }
        [Column("battlePetBreedData")]
        public int BattlePetBreedData { get; set; }
        [Column("battlePetLevel")]
        public int BattlePetLevel { get; set; }
        [Column("battlePetDisplayId")]
        public int BattlePetDisplayId { get; set; }
        [Column("context")]
        public int Context { get; set; }
        [Column("bonusListIDs")]
        public string BonusListIds { get; set; }

        public Character Owner { get; set; }
        public CharacterInventorySlot InventorySlot { get; set; }
        public ItemInstanceTransmog Transmog { get; set; }
    }
}
