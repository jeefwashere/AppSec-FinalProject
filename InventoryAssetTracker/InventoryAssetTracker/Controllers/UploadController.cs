using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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