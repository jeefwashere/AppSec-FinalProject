using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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