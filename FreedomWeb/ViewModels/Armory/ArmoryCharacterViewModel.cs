using FreedomLogic.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreedomWeb.ViewModels.Armory
{
    public class CharacterCustomizationOptionViewModel
    {
        public int OptionId { get; set;  }
        public int ChoiceId { get; set; }
    }

    public class CharacterItemViewModel
    {
        public int Slot { get; set; }
        public int DisplayId { get; set; }
        public int DisplayId2 { get; set; }
        public int ItemVisual { get; set; }
    }

    public class ArmoryCharacterViewModel
    {
        public int CharacterId { get; set;  }
        public string Name { get; set; }
        public int Class { get; set; }
        public int Gender { get; set; }
        public int Race { get; set; }

        public List<CharacterCustomizationOptionViewModel> CustomizationOptions { get; set; } = new List<CharacterCustomizationOptionViewModel>();
        public List<CharacterItemViewModel> Items { get; set; } = new List<CharacterItemViewModel>();
    }
}
