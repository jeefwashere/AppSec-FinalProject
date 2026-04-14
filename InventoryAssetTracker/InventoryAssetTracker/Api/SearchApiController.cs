using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/

namespace InventoryAssetTracker.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class SearchApiController : ControllerBase
	{
		private readonly UserContext userContext;

		public SearchApiController(UserContext userContext)
		{
			this.userContext = userContext;
		}
		/// <summary>
		/// serach for item
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		[HttpGet("search")]
		public async Task<IActionResult> Search([FromQuery] string? term)
		{
			int? userID = GetCurrentUserID();

			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
			//query object 
			IQueryable<Asset> query = userContext.Assets.Where(asset => asset.OwnerId == userID.Value);

			if (!string.IsNullOrWhiteSpace(term))
			{
				string trimmedTerm = term.Trim();

				query = query.Where(asset =>
					asset.Name.Contains(trimmedTerm) ||
					(asset.Description != null && asset.Description.Contains(trimmedTerm)));
			}

			List<AssetResponseDTO> results = await query
				.OrderBy(asset => asset.Name)
				.Select(asset => new AssetResponseDTO
				{
					AssetId = asset.AssetId,
					AssetName = asset.Name,
					Description = asset.Description,
					Quantity = asset.Quantity,
					OwnerId = asset.OwnerId
				})
				.ToListAsync();

			return Ok(results);
		}

		private int? GetCurrentUserID()
		{
			string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!int.TryParse(userIdClaim, out int userId))
			{
				return null;
			}

			return userId;
		}
	}
}
