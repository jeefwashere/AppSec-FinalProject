using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Serves the admin dashboard view
        /// </summary>
        /// <returns>Returns dashboard view</returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Serves the users admin dashboard view
        /// </summary>
        /// <returns>Returns admin user dashboard view</returns>
        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// Serves the assets admin view
        /// </summary>
        /// <returns>Returns the assets admin dashboard view</returns>
        [HttpGet]
        public IActionResult Assets()
        {
            return View();
        }
        
        /// <summary>
        /// Serves the logs admin view 
        /// </summary>
        /// <returns>Returns the logs admin dashboard view</returns>
        [HttpGet]
        public IActionResult Logs()
        {
            return View();
        }
    }
}