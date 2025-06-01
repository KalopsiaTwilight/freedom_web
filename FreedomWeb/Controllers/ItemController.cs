using AspNetCoreGeneratedDocument;
using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomUtils.DataTables;
using FreedomWeb.ViewModels.Item;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{
    public class ItemController : Controller
    {
        DbFreedom _freedomDb;

        public ItemController(DbFreedom dbFreedomDb)
        {
            _freedomDb= dbFreedomDb;
        }

        public IActionResult Index()
        {
            return Redirect(Url.Action("NpcItemData", "Item"));
        }

        public IActionResult NpcItemData()
        {
            return View();
        }

        public IActionResult BonusIdInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> NpcItemData([FromQuery] string searchBy, DTParameterModel parameters)
        {
            var search = parameters.Search.Value ?? "";

            IQueryable<NpcItemInfo> dataQuery;
            if (searchBy == "item")
            {
                dataQuery = _freedomDb.NpcItemInfos
                    .Where(x => x.ItemTemplateExtra.Name.ToUpper().Contains(search.ToUpper())
                             || x.ItemId.ToString().Contains(search));
            } else
            {
                dataQuery = _freedomDb.NpcItemInfos
                    .Where(x => x.CreatureName.ToUpper().Contains(search.ToUpper())
                             || x.CreatureId.ToString().Contains(search));
            }


                
            var data = await dataQuery
                .Select(x => new NpcItemDataViewModel
                {
                    CreatureName = x.CreatureName,
                    CreatureId = x.CreatureId,
                    ItemId = x.ItemId,
                    ItemName = x.ItemTemplateExtra.Name,
                    ItemInventoryType = x.ItemTemplateExtra.InventoryType
                })
                .ApplyDataTableParameters(parameters)
                .ToListAsync();

            var total = _freedomDb.NpcItemInfos.Count();

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = dataQuery.Count(),
                data = data
            });
        }

        [HttpPost]
        public async Task<JsonResult> BonusIdInfo(DTParameterModel parameters)
        {
            var search = parameters.Search.Value ?? "";

            var dataQuery = _freedomDb.ItemBonusIdInfos
                .Where(x => x.ItemTemplateExtra.Name.ToUpper().Contains(search.ToUpper())
                         || x.ItemId.ToString().Contains(search))
                .Select(x => new BonusIdInfoViewModel
                {
                    ItemId = x.ItemId,
                    ItemName = x.ItemTemplateExtra.Name,
                    ItemAppearanceModifierId = x.ItemAppearanceModifierId,
                    BonusId = x.BonusId,
                });
            var data = await dataQuery
                .ApplyDataTableParameters(parameters)
                .ToListAsync();

            var total =await _freedomDb.ItemBonusIdInfos.CountAsync();

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = dataQuery.Count(),
                data = data
            });
        }
    }
}
