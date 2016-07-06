using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.ViewModels.Admin
{
    public class SetGameAccessViewModel
    {
        [Display(Name = "FieldTargetAccountUsername", ResourceType = typeof(AccountRes))]
        public string Username { get; set; }

        public int GameAccId { get; set; }

        [Display(Name = "FieldGMLevelList", ResourceType = typeof(AccountRes))]
        public GMLevel AccountAccess { get; set; }

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
    }
}