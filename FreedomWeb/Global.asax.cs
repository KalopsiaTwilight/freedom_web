using FreedomWeb.Controllers;
using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FreedomWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = "";
            var currentAction = "";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !string.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !string.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            //var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "Oops";
            var code = ErrorCode.InternalServerError;

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                if (Enum.IsDefined(typeof(ErrorCode), httpEx.ErrorCode))
                    code = (ErrorCode)httpEx.ErrorCode;
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.CacheControl = "private";

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            routeData.Values["code"] = code;
            routeData.Values["errInfo"] = new HandleErrorInfo(ex, currentController, currentAction);

            IController errormanagerController = new ErrorController();
            HttpContextWrapper wrapper = new HttpContextWrapper(httpContext);
            var rc = new RequestContext(wrapper, routeData);
            errormanagerController.Execute(rc);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
