/*
 * FILE : AdminController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles admin pages for dashboard, users, assets, and logs.
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/

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
