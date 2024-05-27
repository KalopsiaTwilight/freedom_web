using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FreedomWeb.Controllers
{
    public class ToolsController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ToolsController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult CustomItem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateOrUpdateCustomItem([FromQuery] int? id, [FromBody] WowHeadItemData itemData)
        {
            if (id == null)
            {
                id = GenerateCustomItemId();
            }
            _memoryCache.Set(FreedomWebConstants.CustomItemCacheKeyPrefix + id, itemData, TimeSpan.FromMinutes(5));
            return Json(new
            {
                Id = id
            });
        }

        private int GenerateCustomItemId ()
        {
            string idStr = "9";
            for (int i = 0; i < 5;i++)
            {
                idStr += Math.Floor(Random.Shared.NextDouble() * 10);
            }
            return int.Parse(idStr);
        }
    }
}
