using FreedomLogic.Managers;
using FreedomWeb.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreedomWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ServerControl _serverControl;

        public HomeController(ServerControl serverControl)
        {
            _serverControl = serverControl;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Status()
        {
            var model = new StatusViewModel();
            bool bnetServerRunning = _serverControl.IsBnetServerRunning();
            bool worldServerRunning = _serverControl.IsWorldServerRunning();
            bool worldServerOnline = _serverControl.IsWorldServerOnline();

            if (!bnetServerRunning && !worldServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.Offline;
            }
            else if (worldServerRunning && !worldServerOnline)
            {
                model.Status = EnumFreedomGameserverStatus.WorldLoading;
            }
            else if (!worldServerRunning && bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.WorldDown;
            }
            else if (worldServerRunning && !bnetServerRunning)
            {
                model.Status = EnumFreedomGameserverStatus.LoginDown;
            }
            else
            {
                model.Status = EnumFreedomGameserverStatus.Online;
            }
            return View(model);
        }
    }
}