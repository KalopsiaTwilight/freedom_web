using FreedomLogic.Resources;
using System.ComponentModel.DataAnnotations;

namespace FreedomWeb.ViewModels.Admin
{
    public class PasswordResetTokenViewModel
    {
        [Display(Name = "FieldTargetAccountUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }
        public string ResetLink { get; set; }
    }
}