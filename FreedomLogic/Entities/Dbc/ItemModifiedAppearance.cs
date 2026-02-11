using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbc
{
    [Table("itemmodifiedappearance")]
    public class ItemModifiedAppearance
    {
        [Key]
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int ItemAppearanceModifierID { get; set; }
        public int ItemAppearanceID { get; set; }
        public int OrderIndex { get; set; }
        public byte TransmogSourceTypeEnum { get; set; }

        public Item Item { get; set; }
        public ItemAppearance ItemAppearance { get; set; }
    }
}
