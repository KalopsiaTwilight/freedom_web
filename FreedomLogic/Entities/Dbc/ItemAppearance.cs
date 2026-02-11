using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbc
{
    [Table("itemappearance")]
    public class ItemAppearance
    {
        [Key]
        public int ID { get; set; }
        public int DisplayType { get; set; }
        public int ItemDisplayInfoID { get; set; }
        public int DefaultIconFileDataID { get; set; }
        public int UiOrder { get; set; }
        public int TransmogPlayerConditionID { get; set; }

        public List<ItemModifiedAppearance> ItemModifiedAppearance { get; set;  }
    }

}
