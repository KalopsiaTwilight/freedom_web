using FreedomLogic.Managers;
using FreedomUtils.DataTables;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{ 
    [FreedomAuthorize]
    public class DataController : FreedomController
    {
        /// <summary>
        /// Command List data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        public JsonResult CommandListData(DTParameterModel parameters)
        {
            var user = GetCurrentUser();

            int total = 0;
            int filtered = 0;
            var list = ServerManager.DTGetFilteredAvailableFreedomCommands(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    user.UserData.GameAccountAccess.GMLevel
                );

            return Json(new DTResponseModel() {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = filtered,
                data = list
            });
        }
    }
}