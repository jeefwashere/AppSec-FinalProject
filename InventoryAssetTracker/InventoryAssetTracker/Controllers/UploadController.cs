using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//upload UI pages profile picture
namespace InventoryAssetTracker.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
