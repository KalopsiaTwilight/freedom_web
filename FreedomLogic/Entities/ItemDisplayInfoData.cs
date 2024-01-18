using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("item_to_displayid")]
    public class ItemDisplayInfoData : EntityBase
    {
        [Column("itemId")]
        public override int Id { get; set; }
        [Column("itemName")]
        public string ItemName { get; set; }
        [Column("itemDisplayId")]
        public int DisplayId { get; set; }
        [Column("inventoryType")]
        public int InventoryType { get; set; }
    }
}
