using FreedomLogic.Entities;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreedomWeb.ViewModels.Admin
{

    public class BanGameAccountsViewModel
    {
        [Display(Name = "FieldTargetAccountUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }
        public int UserId { get; set; }

        public int BnetAccountId { get; set; }

        [Display(Name = "Ban Duration")]
        public BanDuration Duration { get; set; }

        [Required]
        [MaxLength(255)]
        public string Reason { get; set; }

        public List<SelectListItem> BanDurationList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (BanDuration duration in Enum.GetValues(typeof(BanDuration)))
                {
                    list.Add(new SelectListItem()
                    {
                        Value = ((int)duration).ToString(),
                        Text = duration.DisplayName(),
                        Selected = duration == Duration,
                    });
                }

                return list;
            }
        }
    }
}
