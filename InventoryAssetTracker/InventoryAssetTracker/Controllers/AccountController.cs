using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
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

            HttpClient client = httpClientFactory.CreateClient();

            LoginRequestDTO request = new LoginRequestDTO
            {
                Username = model.Username,
                Password = model.Password,
                ReturnUrl = model.ReturnUrl
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:7126/api/authapi/login", request);

            if (!response.IsSuccessStatusCode)
            {
                AuthResponseDTO? failedResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

                ModelState.AddModelError(string.Empty, failedResponse?.Message ?? "Login failed.");
                return View(model);
            }

            AuthResponseDTO? successResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            if (successResponse != null && !string.IsNullOrWhiteSpace(successResponse.RedirectUrl))
            {
                return Redirect(successResponse.RedirectUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
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

            AuthResponseDTO? successResponse = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            if (successResponse != null && !string.IsNullOrWhiteSpace(successResponse.RedirectUrl))
            {
                return Redirect(successResponse.RedirectUrl);
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            ProfileViewModel model = new ProfileViewModel
            {
                Username = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                Role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            HttpClient client = httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.PostAsync("https://localhost:7126/api/authapi/logout", null);

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}