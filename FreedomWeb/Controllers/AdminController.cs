using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Admin;
using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize(Roles = FreedomRole.RoleAdmin)]
    public class AdminController : FreedomController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new AdminCPViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult UserList()
        {
            var model = new UserListViewModel();
            return View(model);
        }

        [HttpGet]
        public ActionResult SetGameAccess(int? id)
        {
            var user = UserManager.GetByKey(id ?? 0);
            if (user == null)
            {
                return RedirectToError(ErrorCode.BadRequest);
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

            AccountManager.SetGameAccAccessLevel(model.GameAccId, model.AccountAccess);
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
            bool worldServerRunning = ServerManager.Control.IsWorldServerRunning();
            bool bnetServerRunning = ServerManager.Control.IsBnetServerRunning();
            bool serverOnline = ServerManager.Control.IsWorldServerOnline();

            model.ServerDirectoryPath = SettingsManager.GetServerDir();
            model.WorldServerPath = SettingsManager.GetWorldServerPath();
            model.BnetServerPath = SettingsManager.GetBnetServerPath();
            model.WorldServerPid = worldServerRunning ? ServerManager.Control.GetWorldServerPid() : 0;
            model.BnetServerPid = bnetServerRunning ? ServerManager.Control.GetBnetServerPid() : 0;
            model.WorldServerStatus = worldServerRunning ? (serverOnline ? EnumServerAppStatus.Online : EnumServerAppStatus.Loading) : EnumServerAppStatus.Offline;
            model.BnetServerStatus = bnetServerRunning ? EnumServerAppStatus.Online : EnumServerAppStatus.Offline;
        }

        [HttpPost]
        public JsonResult ServerControlActions()
        {
            bool worldServerRunning = ServerManager.Control.IsWorldServerRunning();
            bool bnetServerRunning = ServerManager.Control.IsBnetServerRunning();

            return Json(new { worldServerRunning = worldServerRunning, bnetServerRunning = bnetServerRunning});
        }

        [HttpPost]
        public JsonResult ServerControlStopWorldServer()
        {
            string error;
            bool stopSuccessful = ServerManager.Control.StopWorldServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStopBnetServer()
        {
            string error;
            bool stopSuccessful = ServerManager.Control.StopBnetServer(out error);

            return Json(new { status = stopSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartWorldServer()
        {
            string error;
            bool startSuccessful = ServerManager.Control.StartWorldServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }

        [HttpPost]
        public JsonResult ServerControlStartBnetServer()
        {
            string error;
            bool startSuccessful = ServerManager.Control.StartBnetServer(out error);

            return Json(new { status = startSuccessful, error = error });
        }
    }
}