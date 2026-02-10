using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Dbo
{
    public enum DboRace
    {
        Human = 0,
        Namekian = 1,
        Majin = 2,
    }

    public class DboCharacter
    {
        [Column("CharID")]
        [Key]
        public int Id { get; set; }
        [Column("CharName")]
        public string Name { get; set; }
        [Column("AccountId")]
        public int AccountId { get; set; }
        [Column("Race")]
        public DboRace Race { get; set; }
        [Column("Adult")]
        public bool IsAdult { get; set; }
        [Column("Level")]
        public int Level { get; set; }
        [Column("Gamemaster")]
        public DboAccountLevel AccountLevel { get; set; }
    }
}
