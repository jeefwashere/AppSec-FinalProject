using Microsoft.AspNetCore.Mvc;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/
namespace InventoryAssetTracker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}