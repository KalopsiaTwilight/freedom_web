using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Freedom
{
    [Table("character_extra")]
    public class CharacterExtra
    {
        [Column("guid")]
        [Key]
        public int Id { get; set; }

        [Column("phase")]
        public int Phase { get; set; }
    }
}
