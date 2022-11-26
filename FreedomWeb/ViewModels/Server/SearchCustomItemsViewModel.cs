using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomUtils.MvcUtils;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FreedomWeb.ViewModels.Server
{
    public class SearchCustomItemsViewModel
    {
        public SearchCustomItemsViewModel()
        {
            SearchType = EnumCustomItemSearchType.CreatureEntryId;
        }

        [Display(Name = "FieldSearchId", ResourceType = typeof(ServerRes))]
        public int? SearchId { get; set; }

        [Display(Name = "FieldEnumSearchType", ResourceType = typeof(ServerRes))]
        public EnumCustomItemSearchType SearchType { get; set; }

        public List<SelectListItem> SearchTypeList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (EnumCustomItemSearchType searchType in Enum.GetValues(typeof(EnumCustomItemSearchType)))
                {
                    list.Add(new SelectListItem()
                    {
                        Value = ((int)searchType).ToString(),
                        Text = searchType.DisplayName(),
                        Selected = searchType == SearchType,
                    });
                }

                return list;
            }
        }
    }
}