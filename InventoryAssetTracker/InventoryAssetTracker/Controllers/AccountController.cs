using InventoryAssetTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    // login/logout controller
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index(string returnUrl = null)
        {
            LoginViewModel login = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };
            return View(login);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
