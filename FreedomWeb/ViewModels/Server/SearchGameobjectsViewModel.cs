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
    public class SearchGameobjectsViewModel
    {
        public SearchGameobjectsViewModel()
        {
            SearchType = EnumGameobjectSearchType.GameobjectEntryId;
        }

        [Display(Name = "FieldSearchId", ResourceType = typeof(ServerRes))]
        public int? SearchId { get; set; }

        [Display(Name = "FieldEnumSearchType", ResourceType = typeof(ServerRes))]
        public EnumGameobjectSearchType SearchType { get; set; }

        public List<SelectListItem> SearchTypeList
        {
            get
            {
                var list = new List<SelectListItem>();

                foreach (EnumGameobjectSearchType searchType in Enum.GetValues(typeof(EnumGameobjectSearchType)))
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