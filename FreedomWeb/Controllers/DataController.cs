using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
using FreedomLogic.Infrastructure;
using FreedomLogic.Managers;
using FreedomLogic.Services;
using FreedomUtils.DataTables;
using FreedomUtils.MvcUtils;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Admin;
using FreedomWeb.ViewModels.Home;
using FreedomWeb.ViewModels.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FreedomWeb.Controllers
{ 
    [Authorize]
    public class DataController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly CharacterManager _characterManager;
        private readonly ServerControl _serverControl;
        private readonly ExtraDataLoader _dataLoader;
        private readonly DbFreedom _freedomDb;
        private readonly AppConfiguration _appConfig;

        public DataController(UserManager<User> userManager, CharacterManager characterManager, ServerControl serverControl, 
            ExtraDataLoader dataLoader, DbFreedom freedomDb, AppConfiguration appConfig)
        {
            _userManager = userManager;
            _characterManager = characterManager;
            _serverControl = serverControl;
            _dataLoader = dataLoader;
            _freedomDb = freedomDb;
            _appConfig = appConfig;
        }



        [HttpGet]
        [Route("/Data/tiles/{dir}/{zoom}/{x}/{y}")]
        public IActionResult GetTile([FromRoute] string dir, [FromRoute] int zoom, [FromRoute] int x, [FromRoute] int y)
        {
            var filePath = Path.Join(_appConfig.Maps.TileRootFolder, dir, zoom.ToString(), $"{y}_{x}.png");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileStream = System.IO.File.Open(filePath, FileMode.Open);
            return File(fileStream, "image/png");
        }

        [HttpGet]
        public IActionResult Maps(string search)
        {
            var maps = Directory.GetDirectories(_appConfig.Maps.TileRootFolder)
                .Where(x => string.IsNullOrEmpty(search) || Path.GetFileName(x).ToLower().Contains(search.ToLower()))
                .Select(Path.GetFileName)
                .ToList();

            return Json(maps);
        }

        /// <summary>
        /// Command List data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<JsonResult> CommandListData(DTParameterModel parameters)
        {
            var currentUser = await _userManager.FindByIdAsync(CurrentUserId);
            _dataLoader.LoadExtraUserData(currentUser);
            var gmLevel = currentUser.UserData.GameAccountAccess.GMLevel;
            if (User.IsInRole(FreedomRole.RoleAdmin))
            {
                gmLevel = GMLevel.Unused;
            }

            var search = parameters.Search.Value ?? "";
            var matchingGmLevels = Enum.GetValues<GMLevel>()
                .Where(l => l.DisplayName().ToUpper().Contains(search))
                .ToArray();

            // Set up basic filter query parts
            var query = _freedomDb.Commands
                .Where(c => ((int)c.GMLevel) <= ((int)gmLevel))
                .Where(c => c.Command.ToUpper().Contains(search.ToUpper())
                         || c.Syntax.ToUpper().Contains(search.ToUpper())
                         || c.Description.ToUpper().Contains(search.ToUpper())
                         || matchingGmLevels.Contains(c.GMLevel)
                );

            // Load and set results
            var data = await query.ApplyDataTableParameters(parameters).ToListAsync();
            var total = _freedomDb.Commands.Where(c => ((int)c.GMLevel) <= ((int)gmLevel)).Count();

            return Json(new DTResponseModel() {
                draw = parameters.Draw,
                recordsTotal = total,
                recordsFiltered = await query.CountAsync(),
                data = data
            });
        }

        /// <summary>
        /// Online character list data source
        /// </summary>
        /// <param name="parameters">DT sent parameters</param>
        /// <param name="filter">DT sent custom filter parameters</param>
        /// <returns></returns>        
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> OnlineListData([FromForm] DTParameterModel parameters)
        {
            var user = await _userManager.FindByIdAsync(CurrentUserId);       
            bool allowUsernameViewing = (user == null ? false : user.FreedomRoles.Where(r => r.Name == FreedomRole.RoleAdmin).Any());

            int total = 0;
            int filtered = 0;
            var charList = _characterManager.DTGetFilteredOnlineCharacters(
                    ref total,
                    ref filtered,
                    parameters.Start,
                    parameters.Length,
                    parameters.Columns,
                    parameters.Order,
                    allowUsernameViewing,
                    parameters.Search.Value ?? ""
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
                    MapName = !_characterManager.IsGMOn(character.Id) ? character.CharData.MapName : (allowUsernameViewing ? "(" + character.CharData.MapName + ")" : "(Hidden)"), //Kret
                    ZoneName = !_characterManager.IsGMOn(character.Id) ? character.CharData.ZoneName : (allowUsernameViewing ? "(" + character.CharData.ZoneName + ")" : "(Hidden)"), //Kret
                    Latency = character.Latency,
                    Phase = character.CharData.Phase
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
        [AllowAnonymous]
        public ActionResult StatusLinePartial()
        {
            var model = new StatusViewModel();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool worldServerOnline = _serverControl.IsWorldServerOnline();

            if (!bnetServerRunning && !worldServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.Offline;
            }
            else if (worldServerRunning && !worldServerOnline)
            {
                model.Status = EnumFreedomGameserverStatus.WorldLoading;
            }
            else if (!worldServerRunning && bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.WorldDown;
            }
            else if (worldServerRunning && !bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.LoginDown;
            }
            else
            {
                model.Status = EnumFreedomGameserverStatus.Online;
            }

            return PartialView("_StatusLinePartial", model);
        } 
    }
}