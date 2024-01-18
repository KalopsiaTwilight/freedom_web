using FreedomLogic.DAL;
using FreedomLogic.Entities;
using FreedomLogic.Identity;
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

        public DataController(UserManager<User> userManager, CharacterManager characterManager, ServerControl serverControl, ExtraDataLoader dataLoader, DbFreedom freedomDb)
        {
            _userManager = userManager;
            _characterManager = characterManager;
            _serverControl = serverControl;
            _dataLoader = dataLoader;
            _freedomDb = freedomDb;
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


        [HttpGet]
        public async Task<JsonResult> TextureFiles([FromQuery] string search)
        {
            var data = await _freedomDb
                .TextureFiles
                .Where(x => x.FileName.Contains(search.ToLower()) || x.Id.ToString().Contains(search))
                .Take(5)
                .ToListAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> RandomTextureFile()
        {
            var data = await _freedomDb.TextureFiles
                .FromSql($@"
SELECT r1.fileId, r1.fileName
FROM texturefiles AS r1 
JOIN (SELECT CEIL(RAND() * (SELECT MAX(fileId) FROM texturefiles)) AS fileId) AS r2
WHERE r1.fileId >= r2.fileId
ORDER BY r1.fileId ASC
LIMIT 1")
                .SingleAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> ItemDisplayData([FromQuery] string search)
        {
            var data = await _freedomDb
                .ItemDisplayInfos
                .Where(x => x.ItemName.Contains(search.ToLower()) || x.Id.ToString().Contains(search))
                .OrderBy(x => x.Id)
                .Take(5)
                .ToListAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> ComponentModels([FromQuery] string search)
        {
            var data = await _freedomDb
                .ModelResources
                .Where(x => x.FileName.Contains(search.ToLower()) || x.Id.ToString().Contains(search))
                .Take(5)
                .ToListAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> RandomComponentModel()
        {
            var data = await _freedomDb.TextureFiles
                .FromSql($@"
SELECT r1.fileId, r1.fileName
FROM modelresources AS r1 
JOIN (SELECT CEIL(RAND() * (SELECT MAX(fileId) FROM modelresources)) AS fileId) AS r2
WHERE r1.fileId >= r2.fileId
ORDER BY r1.fileId ASC
LIMIT 1")
                .SingleAsync();
            return Json(data);
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