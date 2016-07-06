using FreedomLogic.Entities;
using FreedomLogic.Managers;
using FreedomWeb.Infrastructure;
using FreedomWeb.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FreedomWeb.Controllers
{
    [FreedomAuthorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Status()
        {
            var model = new StatusViewModel();
            return View(model);
        }
    }
}