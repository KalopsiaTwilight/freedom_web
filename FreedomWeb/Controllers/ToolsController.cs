using Microsoft.AspNetCore.Mvc;

namespace FreedomWeb.Controllers
{
    public class ToolsController : Controller
    {
        public IActionResult CustomItem()
        {
            return View();
        }
    }
}
