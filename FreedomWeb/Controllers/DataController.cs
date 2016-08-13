using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomUtils.DataTables;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Admin;
using FreedomWeb.ViewModels.Home;
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

        /// <summary>
        /// Online character list data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        public JsonResult OnlineListData(DTParameterModel parameters)
        {
            var user = GetCurrentUser();
            bool allowUsernameViewing = user.FreedomRoles.Where(r => r.Name == FreedomRole.RoleAdmin).Any();

            int total = 0;
            int filtered = 0;
            var charList = CharacterManager.DTGetFilteredOnlineCharacters(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    allowUsernameViewing
                );

            var statusCharList = new List<StatusCharListItem>();

            foreach (var character in charList)
            {
                var raceIconPath = character.Gender == CharGender.Male ? character.CharData.RaceData.IconMalePath : character.CharData.RaceData.IconFemalePath;
                statusCharList.Add(new StatusCharListItem()
                {
                    UserId = character.CharData.WebUser.Id,
                    FactionIconPath = character.CharData.RaceData.IconFactionPath,
                    Name = character.Name,
                    Owner = character.CharData.WebUser.DisplayName,
                    OwnerUsername = allowUsernameViewing ? character.CharData.WebUser.UserName : "",
                    Race = character.CharData.RaceData.Name,
                    RaceIconPath = raceIconPath,
                    Class = character.CharData.ClassData.Name,
                    ClassIconPath = character.CharData.ClassData.IconPath,
                    Gender = Enum.GetName(character.Gender.GetType(), character.Gender),
                    MapName = character.CharData.MapName,
                    ZoneName = character.CharData.ZoneName
                });
            }

            return Json(new DTResponseModel()
            {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = filtered,
                data = statusCharList
            });
        }

        [HttpPost]
        public ActionResult StatusLinePartial()
        {
            var model = new StatusViewModel();

            return PartialView("_StatusLinePartial", model);
        } 
    }
}