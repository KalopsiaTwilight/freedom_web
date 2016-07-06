using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model, string prefix = null)
        {
            if (controller == null)
                throw new System.ArgumentNullException("controller");
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");
            if (!string.IsNullOrWhiteSpace(prefix))
                controller.ViewData = new ViewDataDictionary { TemplateInfo = new TemplateInfo { HtmlFieldPrefix = prefix } };

            return RenderRazorViewToString(controller, viewName, model, true);
        }

        /// <summary>
        /// Renders a view to string.
        /// </summary>
        /// <param name="controller">Controller to extend.</param>
        /// <param name="viewName">The name of a view to render.</param>
        /// <param name="model">Model containing data to render.</param>
        /// <returns>Rendered view as string.</returns>
        public static string RenderViewToString(this Controller controller, string viewName, object model)
        {
            return RenderRazorViewToString(controller, viewName, model, false);
        }

        /// <summary>
        /// Renders the razor view to string.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <param name="renderPartialView">If set to <c>true</c> [render partial view].</param>
        /// <returns>String containing rendered view. </returns>
        private static string RenderRazorViewToString(Controller controller, string viewName, object model, bool renderPartialView)
        {
            if (model != null)
                controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = renderPartialView
                    ? ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName)
                    : ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
