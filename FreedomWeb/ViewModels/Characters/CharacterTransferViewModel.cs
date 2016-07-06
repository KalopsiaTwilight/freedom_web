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
            CharacterId = 0;
        }

        [Required]
        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string TargetUsername { get; set; }

        [Required]
        [Display(Name = "FieldTargetAccountPassword", ResourceType = typeof(AccountRes))]
        public string TargetUserPassword { get; set; }

        [Display(Name = "FieldCharacterToTransfer", ResourceType = typeof(CharacterRes))]
        public int CharacterId { get; set; }

        public List<SelectListItem> CharacterSelectList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var user = UserManager.GetByUsername(TargetUsername.Trim());
            if (user == null)
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferAccountNotFound, new[] { "TargetUsername" });
            }       
            else if (user.BnetAccPassHash.ToUpper() != AccountManager.BnetAccountCalculateShaHash(user.UserName, TargetUserPassword))
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferInvalidPassword, new[] { "TargetUserPassword" });
            }
            else if (CharacterManager.IsGameAccFull(user.UserData.GameAccount.Id))
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferAccountFull, new[] { "TargetUsername" });
            }

            if (DbManager.GetByKey<Character, DbCharacters>(CharacterId) == null)
            {
                yield return new ValidationResult(ErrorRes.ModelErrCharacterTransferCharDoesNotExist, new[] { "CharacterId" });
            }
        }
    }
}