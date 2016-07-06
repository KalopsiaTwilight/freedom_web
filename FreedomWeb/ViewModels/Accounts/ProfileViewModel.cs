using FreedomLogic.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Accounts
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "FieldUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        [Display(Name = "FieldDisplayName", ResourceType = typeof(AccountRes))]
        public string DisplayName { get; set; }

        [Display(Name = "FieldEmail", ResourceType = typeof(AccountRes))]
        public string RegEmail { get; set; }

        [Display(Name = "FieldCreationDate", ResourceType = typeof(AccountRes))]
        public string CreationDateTime { get; set; }
    }
}