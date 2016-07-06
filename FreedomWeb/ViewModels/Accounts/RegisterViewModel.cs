using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Accounts
{
    public class RegisterViewModel : IValidatableObject
    {
        public RegisterViewModel()
        {
        }

        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 2)]
        [Display(Name = "FieldDisplayName", ResourceType = typeof(AccountRes))]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        [Display(Name = "FieldEmail", ResourceType = typeof(AccountRes))]
        public string RegEmail { get; set; }

        [Required]
        [Display(Name = "FieldPassword", ResourceType = typeof(AccountRes))]
        [Password]
        public string Password { get; set; }

        [Required]
        [Display(Name = "FieldRepeatPassword", ResourceType = typeof(AccountRes))]
        public string RepeatPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserManager.GetByUsername(Username.Trim()) != null)
            {
                yield return new ValidationResult(string.Format(ErrorRes.ModelErrUsernameTaken, Username), new[] { "Username" });
            }
            
            if (Password != RepeatPassword)
            {
                yield return new ValidationResult(ErrorRes.ModelErrPasswordsDoNotMatch, new[] { "RepeatPassword" });
            }       
        }
    }
}