using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreedomUtils.MvcUtils;
using FreedomLogic.Managers;
using System.Threading.Tasks;
using FreedomWeb.Models;
using FreedomLogic.Entities;
using FreedomLogic.DAL;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize]
    public class ServerController : FreedomController
    {
        public ActionResult SearchCustomItems()
        {
            var model = new SearchCustomItemsViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SearchCustomItems(SearchCustomItemsViewModel model)
        {
            var resultModel = new CustomItemsSearchResultViewModel();
            resultModel.SearchId = model.SearchId ?? 0;
            var itemInfoList = await ServerManager.CustomItemSearch(resultModel.SearchId, model.SearchType);
            foreach (var item in itemInfoList)
            {
                resultModel.ResultList.Add(new CustomItemResultListItem()
                {
                    EntryId = item.Id,
                    DisplayId = item.DisplayId,
                    Name = item.Name,
                    Description = item.Description,
                    Class = item.Class,
                    ClassName = ServerManager.GetItemClassName(item.Class),
                    SubClass = item.Subclass,
                    SubClassName = ServerManager.GetItemSubclassName(item.Class, item.Subclass),
                    InventoryType = item.InventoryType,
                    InventoryTypeName = ServerManager.GetItemInventoryTypeName(item.InventoryType)
                });
            }
            return PartialView("_CustomItemsSearchResult", resultModel);
        }

        public ActionResult SearchGameobjects()
        {
            var model = new SearchGameobjectsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchGameobjects(SearchGameobjectsViewModel model)
        {
            var resultModel = new GameobjectsSearchResultViewModel();
            resultModel.SearchId = model.SearchId ?? 0;
            var gameobjectInfoList = ServerManager.GameobjectSearch(resultModel.SearchId, model.SearchType);
            foreach (var gob in gameobjectInfoList)
            {
                resultModel.ResultList.Add(new GameobjectResultListItem()
                {
                    EntryId = gob.Id,
                    Type = gob.Type,
                    TypeName = ServerManager.GetGameobjectTypeName(gob.Type),
                    DisplayId = gob.DisplayId,
                    Name = gob.Name,
                    Size = gob.Size
                });
            }
            return PartialView("_GameobjectsSearchResult", resultModel);
        }

        [HttpGet]
        public ActionResult CommandList()
        {
            return View();
        }
    }
}