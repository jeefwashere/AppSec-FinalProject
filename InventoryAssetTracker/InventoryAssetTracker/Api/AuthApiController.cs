/*
 * FILE : AuthApiController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles authentication requests for user login, registration, and logout.
 // Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/*/
using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.Models;
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
		/// <summary>
		/// allow no authorization access for login
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("login")]// post at login    // this will passing a form body be a paramter and bliding then into a loginrequestdto type login
		public async Task<IActionResult> Login([FromBody] LoginRequestDTO login)
		{ //if the model state is not valid, return bad request with a message
			if (!ModelState.IsValid)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid login request."
				});
			}
			
			// Found the corresponding user in database by username, if not found, return unauthorized with a message
			User? user = await userContext.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

			if (user == null)
			{
				return Unauthorized(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid user."
				});
			}
			// Verify login by hash password
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
			// This was to store the cilent infomation like username, email, role and user id in claim, then use cookie to store the claim for authentication
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
			};

			// Add an client identity
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			// Set the cookie authentication properties, like expiration time for 30
			AuthenticationProperties authProperties = new AuthenticationProperties
			{
				IsPersistent = false,
				ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
			};

			// Write your claim into cookie for authentication
			await HttpContext.SignInAsync(
				CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity),
				authProperties);

            string redirectUrl = string.IsNullOrWhiteSpace(login.ReturnUrl) ? "/Item/Index" : login.ReturnUrl;

            return Ok(new AuthResponseDTO
			{
				Success = true,
				Message = "Login successful.",
				RedirectUrl = redirectUrl
			});
		}
		// Register an account
		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterRequestDTO register)
		{
			// Login in infomation validation, if not valid, return bad request with a message
			if (!ModelState.IsValid)
			{
				return BadRequest(new AuthResponseDTO
				{
					Success = false,
					Message = "Invalid registration request."
				});
			}

			// Check is it exist
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
			
			// Create a new user and hash the password, then save the user into database
			User user = new User
			{
				Username = register.Username,
				Email = register.Email,
				Role = "User"
			};

			PasswordHasher<User> hasher = new PasswordHasher<User>();
			string hash = hasher.HashPassword(user, register.Password);

			user.PasswordHash = hash;
			
			// Add the new info
			await userContext.Users.AddAsync(user);
			await userContext.SaveChangesAsync();//save change

			return Ok(new AuthResponseDTO
			{
				Success = true,
				Message = "User registered successfully",
				RedirectUrl = "/Account/Login"
			});
		}
		/// <summary>
		/// log out logic, it will clear the cookie for authentication and return ok with a message and redirect url
		/// </summary>
		/// <returns>Returns a status code containing a message</returns>
		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return Ok(new AuthResponseDTO
			{
				Success = true,
				Message = "Logged out successfully",
				RedirectUrl = "/Home/Index"
			});
		}
	}
}
