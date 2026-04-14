/*
 * FILE : UploadController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles upload pages for file submission and success views.
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/

namespace InventoryAssetTracker.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        /// <summary>
        /// Serves the upload page view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Serves the upload success status page view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}
