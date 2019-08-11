using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.ViewModels.Characters
{
    public class CharacterTransferViewModel : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CharacterManager.IsGameAccFull(AccountId))
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferAccountFull, new[] { "TargetUsername" });
            }

            if (DbManager.GetByKey<Character, DbCharacters>(CharacterId) == null)
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferCharDoesNotExist, new[] { "CharacterId" });
            }

            if (DbManager.GetByKey<GameAccount, DbAuth>(AccountId) == null)
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferAccountNotFound, new[] { "AccountId" });
            }
        }
    }
}