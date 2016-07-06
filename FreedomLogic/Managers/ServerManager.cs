using FreedomLogic.DAL;
using FreedomLogic.DTFilters;
using FreedomLogic.Entities;
using FreedomLogic.Resources;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using FreedomUtils.LINQ;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FreedomLogic.Managers
{
    public enum EnumCustomItemSearchType
    {
        [Display(Name = "ListItemEnumSearchTypeCreatureEntryId", ResourceType = typeof(ServerRes))]
        CreatureEntryId,

        [Display(Name = "ListItemEnumSearchTypeCreatureDisplayId", ResourceType = typeof(ServerRes))]
        CreatureDisplayId,

        [Display(Name = "ListItemEnumSearchTypeItemEntryId", ResourceType = typeof(ServerRes))]
        ItemEntryId,

        [Display(Name = "ListItemEnumSearchTypeItemDisplayId", ResourceType = typeof(ServerRes))]
        ItemDisplayId
    }

    public static class ServerManager
    {
        #region CUSTOM_ITEM_SEARCH        
        private const string WowheadUrlItemConst = "http://www.wowhead.com/item={0}&xml";
        private const int CustomItemIdStartConst = 200000;

        /// <summary>
        /// Search via creature entry id from local database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static async Task<List<CustomItemInfo>> CustomItemSearchByCreatureEntryId(int id)
        {
            var list = new List<CustomItemInfo>();
            var creatureTemplate = DbManager.GetByKey<CreatureTemplate, DbWorld>(id);

            // if no such creature entry was found
            if (creatureTemplate == null)
                return list;

            var creatureEquipTemplate = DbManager.GetByKey<CreatureEquipTemplate, DbWorld>(creatureTemplate.Id);

            // search creature template model ids
            using (var db = new DbFreedom())
            {
                var creatureDisplayInfoList = db.CreatureDisplayInfos
                    .Where(e =>
                        e.Id == creatureTemplate.ModelId1 ||
                        e.Id == creatureTemplate.ModelId2 ||
                        e.Id == creatureTemplate.ModelId3 ||
                        e.Id == creatureTemplate.ModelId4).ToList();


                foreach (var creatureDisplayInfo in creatureDisplayInfoList)
                {
                    // item display - head
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayHead).ToList());

                    // item display - shoulder
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayShoulder).ToList());

                    // item display - shirt
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayShirt).ToList());

                    // item display - cuirass
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayCuirass).ToList());

                    // item display - belt
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayBelt).ToList());

                    // item display - legs
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayLegs).ToList());

                    // item display - boots
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayBoots).ToList());

                    // item display - wrist
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayWrist).ToList());

                    // item display - gloves
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayGloves).ToList());

                    // item display - tabard
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayTabard).ToList());

                    // item display - cape
                    list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayCape).ToList());
                }
            }

            // search creature equip template item ids if creature had such a template
            if (creatureEquipTemplate != null)
            {
                list.AddRange(await CustomItemSearchByItemEntryId(creatureEquipTemplate.ItemIdMainHand));
                list.AddRange(await CustomItemSearchByItemEntryId(creatureEquipTemplate.ItemIdOffHand));
                list.AddRange(await CustomItemSearchByItemEntryId(creatureEquipTemplate.ItemIdRanged));
            }

            // remove duplicates
            list = list.GroupBy(e => e.Id).Select(g => g.First()).ToList();

            return list;
        }

        /// <summary>
        /// Search via creature display id from local database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static List<CustomItemInfo> CustomItemSearchByCreatureDisplayId(int id)
        {
            var list = new List<CustomItemInfo>();

            var creatureDisplayInfo = DbManager.GetByKey<CreatureDisplayInfo, DbFreedom>(id);

            if (creatureDisplayInfo == null)
                return list;

            // search creature template model ids
            using (var db = new DbFreedom())
            {                
                // item display - head
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayHead).ToList());

                // item display - shoulder
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayShoulder).ToList());

                // item display - shirt
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayShirt).ToList());

                // item display - cuirass
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayCuirass).ToList());

                // item display - belt
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayBelt).ToList());

                // item display - legs
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayLegs).ToList());

                // item display - boots
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayBoots).ToList());

                // item display - wrist
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayWrist).ToList());

                // item display - gloves
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayGloves).ToList());

                // item display - tabard
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayTabard).ToList());

                // item display - cape
                list.AddRange(db.CustomItemInfos.Where(i => i.DisplayId == creatureDisplayInfo.ItemDisplayCape).ToList());
            }

            return list;
        }

        /// <summary>
        /// Search via item entry id through WoWhead's XML feeds if it is a default item or through local DB if it is a custom item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static async Task<List<CustomItemInfo>> CustomItemSearchByItemEntryId(int id)
        {
            var list = new List<CustomItemInfo>();

            // skip slow client/connection step if these ids won't be found by wowhead
            if (id == 0)
                return list;

            // search via local database, because item entry id is that of a custom item
            if (id >= CustomItemIdStartConst)
            {
                var item = DbManager.GetByKey<CustomItemInfo, DbFreedom>(id);

                if (item != null)
                {
                    using (var db = new DbFreedom())
                    {
                        list = db.CustomItemInfos.Where(i => i.DisplayId == item.DisplayId).ToList();
                    }
                }

                return list;
            }

            // this is some very hacky crap in attempt to extract display info about npc from wowhead
            try
            {
                using (var client = new WebClient())
                {
                    var url = string.Format(WowheadUrlItemConst, id.ToString());
                    var xmlString = await client.DownloadStringTaskAsync(url);
                    var xmlRoot = XElement.Parse(xmlString);
                    var errorElem = xmlRoot.Element("error");

                    // Wowhead XML indicates that no item was found if it has error tag under wowhead XML element
                    if (errorElem != null)
                        return list;

                    var iconElem = xmlRoot.Element("item").Element("icon");
                    int displayId = 0;

                    // In case displayId is faulty
                    if (!int.TryParse(iconElem.Attribute("displayId").Value, out displayId))
                        return list;

                    using (var db = new DbFreedom())
                    {
                        list = db.CustomItemInfos.Where(i => i.DisplayId == displayId).ToList();
                    }
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (WebException e)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                // TODO low-priority: Implement WebException notification up to controller
            }

            return list;
        }

        /// <summary>
        /// Search via item display id from local database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static List<CustomItemInfo> CustomItemSearchByItemDisplayId(int id)
        {
            using (var db = new DbFreedom())
            {
                return db.CustomItemInfos.Where(i => i.DisplayId == id).ToList();
            }
        }

        public static async Task<List<CustomItemInfo>> CustomItemSearch(int id, EnumCustomItemSearchType searchType)
        {
            var list = new List<CustomItemInfo>();

            if (id != 0)
            {
                switch (searchType)
                {
                    case EnumCustomItemSearchType.CreatureEntryId:
                        list = await CustomItemSearchByCreatureEntryId(id);
                        break;
                    case EnumCustomItemSearchType.CreatureDisplayId:
                        list = CustomItemSearchByCreatureDisplayId(id);
                        break;
                    case EnumCustomItemSearchType.ItemDisplayId:
                        list = CustomItemSearchByItemDisplayId(id);
                        break;
                    case EnumCustomItemSearchType.ItemEntryId:
                        list = await CustomItemSearchByItemEntryId(id);
                        break;
                }
            }

            return list;
        }
        #endregion

        #region CommandList

        public static List<FreedomCommand> GetAvailableFreedomCommands(GMLevel gmlevel)
        {
            using (var db = new DbFreedom())
            {
               return db.FreedomCommands
                    .Where(c => ((int)c.GMLevel) <= ((int)gmlevel))
                    .OrderBy(c => c.Command)
                    .ToList();
            }
        }

        public static List<FreedomCommand> DTGetFilteredAvailableFreedomCommands(
            ref int recordsTotal, 
            ref int recordsFiltered, 
            int start, 
            int length, 
            IEnumerable<DTColumn> columnList,
            IEnumerable<DTOrder> orderList,
            GMLevel gmlevel)
        {
            var list = new List<FreedomCommand>();

            using (var db = new DbFreedom())
            {
                // [0] - Command
                // [1] - Syntax
                // [2] - Description
                // [3] - GMLevel
                var colArray = columnList.ToArray();
                var orderArray = orderList.ToArray();

                // Can't use array in LINQ directly, because: http://stackoverflow.com/a/8354049
                var filterCommand = colArray[0].Search.Value;
                var filterSyntax = colArray[1].Search.Value;
                var filterDescription = colArray[2].Search.Value;
                var filterGmLevelDisplay = colArray[3].Search.Value;

                // Set up basic filter query parts
                var query = db.FreedomCommands
                    .Where(c => ((int)c.GMLevel) <= ((int)gmlevel))
                    .Where(c => c.Command.ToUpper().Contains(filterCommand.ToUpper()))
                    .Where(c => c.Syntax.ToUpper().Contains(filterSyntax.ToUpper()))
                    .Where(c => c.Description.ToUpper().Contains(filterDescription.ToUpper()));

                // Can't filter GMLevel through display value directly, so we enumerate through all GMLevel enums 
                // and filter out non-matching ones from query
                foreach (GMLevel value in Enum.GetValues(typeof(GMLevel)))
                {
                    if (!value.DisplayName().Contains(filterGmLevelDisplay.ToUpper(), StringComparison.OrdinalIgnoreCase))
                        query = query.Where(c => c.GMLevel != value);
                }

                // Column ordering
                bool nextLevel = false;
                IOrderedQueryable<FreedomCommand> orderedQuery = null;

                foreach (var order in orderArray)
                {
                    string colName = colArray[order.Column].Data;
                    if (!nextLevel)
                    {
                        orderedQuery = query.OrderBy(colName, order.Dir == "desc");
                        nextLevel = true;
                    }
                    else
                    {
                        orderedQuery = orderedQuery.ThenBy(colName, order.Dir == "desc");
                        nextLevel = true;
                    }
                }
               
                // Load and set results
                list = orderedQuery.Skip(start).Take(length).ToList();
                recordsTotal = db.FreedomCommands.Where(c => ((int)c.GMLevel) <= ((int)gmlevel)).Count();
                recordsFiltered = orderedQuery.Count();
            }

            return list;
        }

        #endregion

        public static string GetItemClassName(int itemClassId)
        {
            using (var db = new DbFreedom())
            {
                var itemClass = db.ItemClassInfos.Where(i => i.Id == itemClassId).FirstOrDefault();
                if (itemClass != null)
                    return itemClass.Name;
                else
                    return "Unknown";
            }
        }

        public static string GetItemSubclassName(int itemClassId, int itemSubclassId)
        {
            using (var db = new DbFreedom())
            {
                var itemSubclass = db.ItemSubclassInfos.Where(i => i.Id == itemClassId && i.SubclassId == itemSubclassId).FirstOrDefault();
                if (itemSubclass != null)
                    return itemSubclass.Name;
                else
                    return "Unknown";
            }
        }

        public static string GetItemInventoryTypeName(int itemInventoryTypeId)
        {
            using (var db = new DbFreedom())
            {
                var itemInventoryType = db.ItemInventoryTypeInfos.Where(i => i.Id == itemInventoryTypeId).FirstOrDefault();
                if (itemInventoryType != null)
                    return itemInventoryType.Name;
                else
                    return "Unknown";
            }
        }
    }
}
