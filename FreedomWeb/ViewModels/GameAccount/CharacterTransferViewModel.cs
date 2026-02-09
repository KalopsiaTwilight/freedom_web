using FreedomLogic.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreedomWeb.ViewModels.GameAccount
{
    public class CharacterTransferViewModel
    {
        public CharacterTransferViewModel()
        {
            CharacterSelectList = new List<SelectListItem>();
            AccountSelectList = new List<SelectListItem>();
            CharacterId = 0;
            AccountId = 0;
        }

        [Display(Name = "FieldTargetAccount", ResourceType = typeof(AccountRes))]
        public int AccountId { get; set; }

        [Display(Name = "FieldCharacterToTransfer", ResourceType = typeof(CharacterRes))]
        public int CharacterId { get; set; }

        public List<SelectListItem> AccountSelectList { get; set; }
        public List<SelectListItem> CharacterSelectList { get; set; }
    }
}