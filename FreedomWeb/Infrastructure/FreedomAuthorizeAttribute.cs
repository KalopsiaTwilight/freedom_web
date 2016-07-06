using FreedomWeb.Controllers;
using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FreedomWeb.Infrastructure
{
    public class FreedomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", "LogIn" },
                    { "controller", "Account" },
                    { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                });
            }
            else
            {
                filterContext.Controller.TempData[ErrorController.TempDataErrorCodeConst] = ErrorCode.Unauthorized;
                filterContext.Result = new RedirectResult("~/Error/Oops");
            }
        }
    }
}