using FreedomLogic.Entities;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Accounts
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
        }

        [Required]
        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        [Required]
        [Display(Name = "FieldPassword", ResourceType = typeof(AccountRes))]
        public string Password { get; set; }
        
        public string ReturnUrl { get; set; }

        [Display(Name = "FieldRememberMe", ResourceType = typeof(AccountRes))]
        public bool RememberMe { get; set; }
    }
}