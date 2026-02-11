using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Characters
{

    [Table("character_inventory")]
    public class CharacterInventorySlot
    {
        [Column("guid")]
        public int CharacterId { get; set; }
        [Column("bag")]
        public int Bag { get; set; }
        [Column("slot")]
        public int Slot { get; set; }
        [Column("item")]
        [Key]
        public int ItemId { get; set; }

        public Character Character { get; set; }

        public ItemInstance ItemInstance { get; set; }
    }
}
