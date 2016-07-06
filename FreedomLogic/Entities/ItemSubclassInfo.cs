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
    [Table("item_subclass_info")]
    public class ItemSubclassInfo : EntityBase
    {
        [Key]
        [Column("id_class", Order = 1)]
        public override int Id { get; set; }

        [Key]
        [Column("id_subclass", Order = 2)]
        public int SubclassId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("name2")]
        public string Name2 { get; set; }
    }
}
