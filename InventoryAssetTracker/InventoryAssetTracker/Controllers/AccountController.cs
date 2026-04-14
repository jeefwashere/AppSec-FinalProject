/*
 * FILE : AccountController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles account pages for login, register, profile, and logout.
 */
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

        /// <summary>
        /// Serves the login view
        /// </summary>
        /// <param name="returnUrl">The URL the user originated from</param>
        /// <returns>Returns a login view</returns>
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

        /// <summary>
        /// Takes a login view model; Authenticates and logs in user if valid
        /// </summary>
        /// <param name="model">Login view model containing relevant data needed to login</param>
        /// <returns>Returns a view depending if login succeeds</returns>
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

        /// <summary>
        /// Serves a register view
        /// </summary>
        /// <returns>Register view or takes user back to item screen if logged in already</returns>
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

        /// <summary>
        /// Registers the user
        /// </summary>
        /// <param name="model">Contains all relevant registration data for user</param>
        /// <returns>A view depending on the registration status</returns>
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

        /// <summary>
        /// Serves the profile view
        /// </summary>
        /// <returns>A view depending on if the user is logged in or not</returns>
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

        /// <summary>
        /// Signs user out
        /// </summary>
        /// <returns>A redirect view to the home page</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Access denied page
        /// </summary>
        /// <returns>Access denied page</returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
