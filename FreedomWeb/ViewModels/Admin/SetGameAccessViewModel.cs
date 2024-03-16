using FreedomLogic.Entities;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Admin
{
    public class SetGameAccessViewModel
    {
        [Display(Name = "FieldTargetAccountUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }
        public int UserId { get; set; }

        public int BnetAccountId { get; set; }

        [Display(Name = "WoW GMLevel / Game account access")]
        public GMLevel AccountAccess { get; set; }
        [Display(Name = "DBOG GMLevel / Game account access")]
        public DboAccountLevel DboAccountAccess { get; set; }

        public List<SelectListItem> GMLevelList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (GMLevel gmlevel in Enum.GetValues(typeof(GMLevel)))
                {
                    list.Add(new SelectListItem()
                    {
                        Value = ((int)gmlevel).ToString(),
                        Text = gmlevel.DisplayName(),
                        Selected = gmlevel == AccountAccess,
                    });
                }

                return list;
            }
        }

        public List<SelectListItem> DboAccountLevelList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (DboAccountLevel accountLevel in Enum.GetValues(typeof(DboAccountLevel)))
                {
                    list.Add(new SelectListItem()
                    {
                        Value = ((int)accountLevel).ToString(),
                        Text = accountLevel.DisplayName(),
                        Selected = accountLevel == DboAccountAccess,
                    });
                }

                return list;
            }
        }
    }
}