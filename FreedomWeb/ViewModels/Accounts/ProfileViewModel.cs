using FreedomLogic.Resources;
using FreedomUtils.MvcUtils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class EditProfileInfoViewModel
    {
        public int UserId { get; set; }
        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        [Display(Name = "FieldDisplayName", ResourceType = typeof(AccountRes))]
        [StringLength(32, MinimumLength = 2)]
        [Required]
        public string DisplayName { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(AccountRes))]
        [StringLength(255, MinimumLength = 2)]
        [EmailAddress]
        [Required]
        public string RegEmail { get; set; }

        [Display(Name = "FieldCreationDate", ResourceType = typeof(AccountRes))]
        public string CreationDateTime { get; set; }
    }

    public class ProfileViewModel
    {
        public int UserId { get; set; }
        public EditProfileInfoViewModel EditProfileViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}