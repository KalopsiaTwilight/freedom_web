using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(AccountRes))]
        [StringLength(255)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}