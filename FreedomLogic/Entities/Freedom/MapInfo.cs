using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
{
    [Table("map_info")]
    public class MapInfo
    {
        [Column("id")]
        [Key]
        public short Id { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
