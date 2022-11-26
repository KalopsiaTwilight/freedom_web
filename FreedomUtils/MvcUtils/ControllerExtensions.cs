using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace FreedomUtils.MvcUtils
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Renders a (partial) view to string.
        /// </summary>
        /// <param name="controller">Controller to extend.</param>
        /// <param name="viewName">Partial view to render.</param>
        /// <param name="model">Model containing data to render.</param>
        /// <param name="prefix">The prefix for the model, default value is null.</param>
        /// <returns>Rendered (partial) view as string.</returns>
        public static Task<string> RenderPartialViewToStringAsync(this Controller controller, string viewName, object model, string prefix = null)
        {
            if (controller == null)
                throw new System.ArgumentNullException("controller");
            if (string.IsNullOrEmpty(viewName))
                viewName = (string) controller.ControllerContext.RouteData.Values["action"];
            if (!string.IsNullOrWhiteSpace(prefix))
                controller.ViewData.TemplateInfo.HtmlFieldPrefix = prefix; 
            return RenderRazorViewToStringAsync(controller, viewName, model, true);
        }

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="controller">Controller to extend.</param>
        /// <param name="viewName">The name of a view to render.</param>
        /// <param name="model">Model containing data to render.</param>
        /// <returns>Rendered view as string.</returns>
        public static Task<string> RenderViewToStringAsync(this Controller controller, string viewName, object model)
        {
            return RenderRazorViewToStringAsync(controller, viewName, model, false);
        }

        /// <summary>
        /// Renders the razor view to string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="renderPartialView">If set to <c>true</c> [render partial view].</param>
        /// <returns>String containing rendered view. </returns>
        private static async Task<string> RenderRazorViewToStringAsync(Controller controller, string viewName, object model, bool renderPartialView)
        {
            if (model != null)
                controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService<IRazorViewEngine>();
                var viewResult = renderPartialView
                    ? viewEngine.FindView(controller.ControllerContext, viewName, false)
                    : viewEngine.FindView(controller.ControllerContext, viewName, true);

                viewResult.EnsureSuccessful(null);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
                await viewResult.View.RenderAsync(viewContext);

                return sw.ToString();
            }
        }
    }
}
