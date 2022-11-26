using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreedomWeb.ViewModels.Server
{
    public class CustomItemsSearchResultViewModel
    {
        public CustomItemsSearchResultViewModel()
        {
            ResultList = new List<CustomItemResultListItem>();
        }

        public int SearchId { get; set; }

        public List <CustomItemResultListItem> ResultList { get; set; }
    }
}