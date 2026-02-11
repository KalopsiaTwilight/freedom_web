using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbc
{
    [Table("itembonus")]
    public class ItemBonus
    {
        [Key]
        public int ID { get; set; }
        public int Value0 { get; set; }
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }
        public int ParentItemBonusListID { get; set; }
        public byte Type { get; set; }
        public byte OrderIndex { get; set; }
    }

}
