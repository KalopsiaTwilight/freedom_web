using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
{
    [Table("item_custom_info")]
    public class CustomItemInfo : EntityBase
    {
        [Key]
        [Column("id_item")]
        public override int Id { get; set; }

        [Column("id_display")]
        public int DisplayId { get; set; }

        [Column("class")]
        public int Class { get; set; }

        [Column("subclass")]
        public int Subclass { get; set; }

        [Column("inventory_type")]
        public int InventoryType { get; set; }

        [Column("sheath")]
        public int Sheath { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
