using FreedomLogic.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
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
