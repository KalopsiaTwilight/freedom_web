using FreedomLogic.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomLogic.Entities
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
    }
}
