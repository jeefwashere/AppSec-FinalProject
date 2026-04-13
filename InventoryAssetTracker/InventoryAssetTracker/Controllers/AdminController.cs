using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Assets()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logs()
        {
            return View();
        }
    }
}