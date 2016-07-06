using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Infrastructure
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var model = new ErrorViewModel();
            model.Error = ErrorCode.InternalServerError;
            model.ErrInfo = new HandleErrorInfo(ex, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());

            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Error/Error.cshtml",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}