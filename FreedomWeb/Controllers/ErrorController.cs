using FreedomLogic.Resources;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize]
    public class ErrorController : FreedomController
    {
        public const string TempDataErrorCodeConst = "ErrorCode";

        [AllowAnonymous]
        public ActionResult Oops(ErrorCode code = ErrorCode.ErrDefault, HandleErrorInfo errInfo = null)
        {
            // TempData storage of error code will override given error code through URL parameter
            if (TempData[TempDataErrorCodeConst] != null)
                code = (ErrorCode)TempData[TempDataErrorCodeConst];

            var model = new ErrorViewModel();
            model.Error = code;
            model.ErrInfo = errInfo;                      

            return View("Error", model);
        }
    }
}