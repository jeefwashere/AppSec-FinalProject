using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Serves the home page view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Serves the privacy page view
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Serves an error page view
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }
    }
}