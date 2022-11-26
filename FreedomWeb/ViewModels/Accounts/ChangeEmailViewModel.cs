using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class ChangeEmailViewModel
    {
        [Display(Name = "FieldNewEmail", ResourceType = typeof(AccountRes))]
        [StringLength(255)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}