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
using System.Text.RegularExpressions;
using InventoryAssetTracker.Controllers;

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
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
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

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDTO register)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid registration request."
				});
			}

			bool checkUsernameExists = await userContext.Users.AnyAsync(u => u.Username == register.Username);

			if (checkUsernameExists)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Username already exists."
				});
			}

			bool checkEmailExists = await userContext.Users.AnyAsync(u => u.Email == register.Email);

			if (checkEmailExists)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Email already exists."
				});
			}

			User user = new User
			{
				Username = register.Username,
				Email = register.Email,
				Role = "User"
			};

			PasswordHasher<User> hasher = new PasswordHasher<User>();
			string hash = hasher.HashPassword(user, register.Password);

			user.PasswordHash = hash;

			await userContext.Users.AddAsync(user);
			await userContext.SaveChangesAsync();

			return Ok(new AuthResponseDTO
			{
				Success = true,
				Message = "User registered successfully",
				RedirectUrl = "/Account/Index"
			});
		}
	}
}
