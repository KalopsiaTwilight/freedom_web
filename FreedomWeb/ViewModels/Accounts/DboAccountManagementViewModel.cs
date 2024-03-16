using FreedomLogic.Resources;
using FreedomUtils.MvcUtils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class DboAccountManagementViewModel : IValidatableObject
    {
        public int? AccountId { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [MaxLength(15)]
        public string AccountName { get; set; } 

        [Required]
        [Display(Name = "New Password")]
        [MinLength(8)]
        [MaxLength(16)]
        [Password]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Repeat Password")]
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