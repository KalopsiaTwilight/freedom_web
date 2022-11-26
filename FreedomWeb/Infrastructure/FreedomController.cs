using FreedomLogic.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected string GetCurrentUserName => User.FindFirstValue(ClaimTypes.Name);
    }
}