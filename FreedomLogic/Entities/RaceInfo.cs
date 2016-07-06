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
    [Table("race_info")]
    public class RaceInfo : EntityBase
    {
        [Column("id")]
        [Key]
        public override int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("icon_male")]
        public string IconMalePath { get; set; }

        [Column("icon_female")]
        public string IconFemalePath { get; set; }
        
        [Column("icon_faction")]
        public string IconFactionPath { get; set; }
    }
}
