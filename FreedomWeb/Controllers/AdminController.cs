using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Admin;
using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
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
    }
}