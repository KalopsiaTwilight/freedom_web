using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbc
{
    [Table("item")]
    public class Item
    {
        [Key]
        public int ID { get; set; }
        public byte ClassID { get; set; }
        public byte SubclassID { get; set; }
        public byte Material { get; set; }
        public sbyte InventoryType { get; set; }
        public byte SheatheType { get; set; }
        public sbyte Sound_override_subclassID { get; set; }
        public int IconFileDataID { get; set; }
        public byte ItemGroupSoundsID { get; set; }
        public int ContentTuningID { get; set; }
        public int ModifiedCraftingReagentItemID { get; set; }

        public List<ItemModifiedAppearance> ItemModifiedAppearances { get; set; }
    }

}
