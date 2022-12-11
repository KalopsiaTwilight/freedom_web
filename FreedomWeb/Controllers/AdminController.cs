using FreedomLogic.DAL;
using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.Models;
using FreedomWeb.ViewModels.Admin;
using FreedomUtils.MvcUtils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FreedomLogic.Infrastructure;
using System.IO;
using Microsoft.EntityFrameworkCore.Internal;
using FreedomLogic.Services;

namespace FreedomWeb.Controllers
{
    [Authorize(Roles = FreedomRole.RoleAdmin)]
    public class AdminController : FreedomController
    {
        private readonly UserManager<User> _userManager;
        private readonly DbFreedom _freedomDb;
        private readonly AccountManager _accountManager;
        private readonly ServerControl _serverControl;
        private readonly AppConfiguration _appConfig;
        private readonly ExtraDataLoader _dataLoader;

        public AdminController(UserManager<User> userManager, DbFreedom freedomDb, AccountManager accountManager,
            ServerControl serverControl, AppConfiguration appConfig, ExtraDataLoader dataLoader)
        {
            _userManager = userManager;
            _freedomDb = freedomDb;
            _accountManager = accountManager;
            _serverControl = serverControl;
            _appConfig = appConfig;
            _dataLoader = dataLoader;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new AdminCPViewModel
            {
                AdminList = new List<AdminListItem>()
            };
            var adminUsers = _freedomDb.Users
                .Include(u => u.FreedomRoles)
                .Where(u => u.FreedomRoles.Any(r => r.Name == FreedomRole.RoleAdmin))
                .ToList();
            foreach (var admin in adminUsers)
            {
                _dataLoader.LoadExtraUserData(admin);
                var gmLevel = admin.UserData.GameAccountAccess.GMLevel;
                model.AdminList.Add(new AdminListItem()
                {
                    UserId = admin.Id,
                    Username = admin.UserName,
                    DisplayName = admin.DisplayName,
                    Email = admin.RegEmail,
                    Roles = string.Join(", ", admin.FreedomRoles.Select(r => r.Name)),
                    GameAccess = gmLevel.DisplayName(),
                });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            var model = new UserListViewModel();
            model.UserList = new List<UserListItem>();

            var users = _freedomDb.Users
               .Include(u => u.FreedomRoles)
               .ToList();
            foreach (var user in users)
            {
                _dataLoader.LoadExtraUserData(user);
                var gmLevel = user.UserData.GameAccountAccess?.GMLevel ?? GMLevel.Unused;
                model.UserList.Add(new UserListItem()
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Email = user.RegEmail,
                    Roles = string.Join(", ", user.FreedomRoles.Select(r => r.Name)),
                    GameAccess = gmLevel.DisplayName(),
                });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SetGameAccess(int? id)
        {
            var user = await _userManager.FindByIdAsync(id?.ToString());
            _dataLoader.LoadExtraUserData(user);
            if (user == null)
            {
                // TODO: Handle error
                //return RedirectToError(ErrorCode.BadRequest);
            }

            var model = new SetGameAccessViewModel();
            model.Username = user.UserName;
            model.GameAccId = user.UserData.GameAccount.Id;
            model.AccountAccess = user.UserData.GameAccountAccess.GMLevel;
            return View(model);
        }

        [HttpPost]
        public ActionResult SetGameAccess(SetGameAccessViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _accountManager.SetGameAccAccessLevel(model.GameAccId, model.AccountAccess);
            SetAlertMsg(AlertRes.AlertSuccessSetGameAccess, AlertMsgType.AlertSuccess);
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public ActionResult ServerControl()
        {
            var model = new ServerControlViewModel();
            LoadServerControlDataViewModel(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult ServerControlData()
        {
            var model = new ServerControlViewModel();
            LoadServerControlDataViewModel(model);

            return PartialView("_ServerControlData", model);
        }

        private void LoadServerControlDataViewModel(ServerControlViewModel model)
        {
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool serverOnline = _serverControl.IsWorldServerOnline();

            model.ServerDirectoryPath = _appConfig.TrinityCore.ServerDir;
            model.WorldServerPath = Path.Combine(_appConfig.TrinityCore.ServerDir, "worldserver.exe");
            model.BnetServerPath = Path.Combine(_appConfig.TrinityCore.ServerDir, "bnetserver.exe");
            model.WorldServerPid = worldServerRunning ? _serverControl.GetWorldServerPid() : 0;
            model.BnetServerPid = bnetServerRunning ? _serverControl.GetBnetServerPid() : 0;
            model.WorldServerStatus = worldServerRunning ? (serverOnline ? EnumServerAppStatus.Online : EnumServerAppStatus.Loading) : EnumServerAppStatus.Offline;
            model.BnetServerStatus = bnetServerRunning ? EnumServerAppStatus.Online : EnumServerAppStatus.Offline;
        }

        [HttpPost]
        public JsonResult ServerControlActions()
        {
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();

            return Json(new { worldServerRunning, bnetServerRunning });
        }

        [HttpPost]
        public JsonResult ServerControlStopWorldServer()
        {
            string error;
            bool stopSuccessful = _serverControl.StopWorldServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStopBnetServer()
        {
            string error;
            bool stopSuccessful = _serverControl.StopBnetServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartWorldServer()
        {
            string error;
            bool startSuccessful = _serverControl.StartWorldServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartBnetServer()
        {
            string error;
            bool startSuccessful = _serverControl.StartBnetServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }
    }
}