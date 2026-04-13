using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    // mvc UI pages item/list/add/update
    public class ItemController : Controller
    {
       
            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Add()
            {
                return View();
            }

            public IActionResult Update(int id)
            {
                return View();
            }
        }
}
