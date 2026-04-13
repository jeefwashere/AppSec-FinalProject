using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    // admin panel pages
    public class AdminController : Controller
    {
      
            public IActionResult Index()
            {
                return View();
            }
        
    }
}
