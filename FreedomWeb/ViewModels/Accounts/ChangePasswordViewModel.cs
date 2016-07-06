using FreedomLogic.Resources;
using FreedomUtils.MvcUtils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Accounts
{
    public class ChangePasswordViewModel : IValidatableObject
    {
        [Required]
        [Display(Name = "FieldCurrentPassword", ResourceType = typeof(AccountRes))]
        public string CurrentPassword { get; set; }

        [Required]
        [Display(Name = "FieldNewPassword", ResourceType = typeof(AccountRes))]
        [Password]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "FieldRepeatPassword", ResourceType = typeof(AccountRes))]
        public string RepeatPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword != RepeatPassword)
            {
                yield return new ValidationResult(ErrorRes.ModelErrPasswordsDoNotMatch, new[] { "RepeatPassword" });
            }
        }
    }
}