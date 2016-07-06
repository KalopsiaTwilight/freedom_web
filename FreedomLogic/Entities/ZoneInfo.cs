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
    [Table("zone_info")]
    public class ZoneInfo : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
