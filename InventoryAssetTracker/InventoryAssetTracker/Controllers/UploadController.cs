using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/

namespace InventoryAssetTracker.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}