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
    [Table("map_info")]
    public class MapInfo : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
