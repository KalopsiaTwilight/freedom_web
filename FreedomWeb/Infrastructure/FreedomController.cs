using FreedomLogic.Identity;
using FreedomLogic.Managers;
using FreedomWeb.Controllers;
using FreedomWeb.ViewModels.Errors;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Infrastructure
{
    public class FreedomController : Controller
    {
        protected enum AlertMsgType
        {
            AlertSuccess,
            AlertInfo,
            AlertWarning,
            AlertDanger
        }

        protected void SetAlertMsg(string message, AlertMsgType type)
        {
            switch(type)
            {
                case AlertMsgType.AlertSuccess:
                    TempData["AlertMsgSuccess"] = message;
                    break;
                case AlertMsgType.AlertInfo:
                    TempData["AlertMsgInfo"] = message;
                    break;
                case AlertMsgType.AlertWarning:
                    TempData["AlertMsgWarning"] = message;
                    break;
                case AlertMsgType.AlertDanger:
                    TempData["AlertMsgDanger"] = message;
                    break;
                default:
                    break;
            }
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected ActionResult RedirectToError(ErrorCode code)
        {
            TempData[ErrorController.TempDataErrorCodeConst] = code;
            return RedirectToAction("Oops", "Error");
        }

        protected User GetCurrentUser()
        {
            return UserManager.GetByKey(User.Identity.GetUserId<int>());
        }
    }
}