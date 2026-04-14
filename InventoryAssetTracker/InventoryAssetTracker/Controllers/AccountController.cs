using System.Security.Claims;
using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.Models;
using InventoryAssetTracker.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/
namespace InventoryAssetTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly UserContext userContext;

        public AccountController(IHttpClientFactory httpClientFactory, UserContext userContext)
        {
            this.httpClientFactory = httpClientFactory;
            this.userContext = userContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Item");
            }

            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User? user = await userContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            PasswordVerificationResult passwordCheck = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (passwordCheck == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Item");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Item");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpClient client = httpClientFactory.CreateClient();

            RegisterRequestDTO request = new RegisterRequestDTO
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7126/api/authapi/register", request);

            if (!response.IsSuccessStatusCode)
            {
                AuthResponseDTO? failedResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                ModelState.AddModelError(string.Empty, failedResponse?.Message ?? "Registration failed.");
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            User? user = await userContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ProfileViewModel model = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                ProfilePhotoPath = user.ProfilePhotoPath
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}