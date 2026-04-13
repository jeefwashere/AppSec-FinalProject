using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.Models;
using InventoryAssetTracker.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace InventoryAssetTracker.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthApiController : ControllerBase
	{
		private readonly UserContext userContext;

		public AuthApiController(UserContext userContext)
		{
			this.userContext = userContext;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO login)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid login request."
				});
			}

			User? user = await userContext.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

			if (user == null)
			{
				return Unauthorized(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid user."
				});
			}

			PasswordHasher<User> hasher = new PasswordHasher<User>();
			PasswordVerificationResult passwordCheck = hasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

			if (passwordCheck == PasswordVerificationResult.Failed)
			{
				return Unauthorized(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid password."
				});
			}

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
			};

			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			AuthenticationProperties authProperties = new AuthenticationProperties
			{
				IsPersistent = false,
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
			};

			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

			string redirectUrl = string.IsNullOrWhiteSpace(login.ReturnUrl) ? "/Asset/Index" : login.ReturnUrl;

			return Ok(new AuthResponseDTO
			{
				Success = true,
				Message = "Login successful.",
				RedirectUrl = redirectUrl
			});
		}

		[HttpPost("register")]
		public IActionResult Register()
		{
			return Ok();
		}
	}
}
