using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomLogic.Entities.Characters
{

    [Table("character_customizations")]
    public class CharacterCustomization
    {
        [Column("guid")]
        public int CharacterId { get; set; }
        [Column("chrCustomizationOptionID")]
        public int CustomizationOptionId { get; set; }
        [Column("chrCustomizationChoiceID")]
        public int CustomizationChoiceId { get; set; }

        public Character Character { get; set; }
    }
}
