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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Oops(ErrorCode code = ErrorCode.ErrDefault)
        {
            // TempData storage of error code will override given error code through URL parameter
            if (TempData[TempDataErrorCodeConst] != null)
                code = (ErrorCode)TempData[TempDataErrorCodeConst];

            var model = new ErrorViewModel();
            model.Error = code;
                        
            // set response status code in case error was one of the standard web errors
            switch(code)
            {
                case ErrorCode.Unauthorized:
                    SetAlertMsg(ErrorRes.ErrUnauthorized, AlertMsgType.AlertDanger);
                    return RedirectToAction("Index", "Home");
                case ErrorCode.BadRequest:
                    SetAlertMsg(ErrorRes.ErrBadRequest, AlertMsgType.AlertDanger);
                    return RedirectToAction("Index", "Home");
                case ErrorCode.NotFound:
                    SetAlertMsg(ErrorRes.ErrNotFound, AlertMsgType.AlertDanger);
                    return RedirectToAction("Index", "Home");
                case ErrorCode.InternalServerError:
                    Response.StatusCode = (int)code;
                    break;
                default:
                    Response.StatusCode = (int)ErrorCode.ErrDefault;
                    break;
            }

            return View("Error", model);
        }
    }
}