using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class ChangeDisplayNameViewModel
    {
        [Display(Name = "FieldCurrentDisplayName", ResourceType = typeof(AccountRes))]
        public string CurrentDisplayName { get; set; }

        [Display(Name = "FieldNewDisplayName", ResourceType = typeof(AccountRes))]
        [StringLength(32, MinimumLength = 2)]
        [Required]
        public string DisplayName { get; set; }
    }
}