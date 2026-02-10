using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
{
    [Table("zone_info")]
    public class ZoneInfo
    {
        [Column("id")]
        [Key]
        public short Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
