/*
 * FILE : ItemController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles item pages for listing, details, adding, and updating.
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/
namespace InventoryAssetTracker.Controllers
{
    [Authorize]
    public class ItemController : Controller
    {
        /// <summary>
        /// Serves the item list page view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Serves the asset details page
        /// </summary>
        /// <param name="id">Asset ID</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.AssetId = id;
            return View();
        }

        /// <summary>
        /// Serves the add page view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Serves the update page view
        /// </summary>
        /// <param name="id">Asset ID</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.AssetId = id;
            return View();
        }
    }
}
