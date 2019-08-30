using FreedomWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreedomWeb.ViewModels.Server
{
    public class GameobjectsSearchResultViewModel
    {
        public GameobjectsSearchResultViewModel()
        {
            ResultList = new List<GameobjectResultListItem>();
        }

        public int SearchId { get; set; }

        public List <GameobjectResultListItem> ResultList { get; set; }
    }
}