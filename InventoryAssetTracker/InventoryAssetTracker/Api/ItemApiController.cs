/*
 * FILE : ItemApiController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles inventory item requests for viewing, creating, updating, deleting.
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InventoryAssetTracker.Data;
using InventoryAssetTracker.Models;
using InventoryAssetTracker.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;



// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/

namespace InventoryAssetTracker.Api
{
	//api/itemApi
	[Route("api/[controller]")]
	[ApiController]
	[Authorize] // only authorized user can access this controller
	public class ItemApiController : ControllerBase
	{
		private readonly UserContext userContext;

		public ItemApiController(UserContext userContext)
		{
			this.userContext = userContext;
		}
		/// <summary>
		/// get all items for current user, if the user is not authorized, return unauthorized with a message, if the user is authorized, return ok with the list of items
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> GetMyItems()
		{
			int? userID = GetCurrentUserID();

			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
//get the items for current user by userId search inside from the database, then return the list of items
			List<AssetResponseDTO> selectedAssets = await userContext.Assets
				.Where(asset => asset.OwnerId == userID.Value)
				.Select(asset => new AssetResponseDTO
				{
					AssetId = asset.AssetId,
					AssetName = asset.Name,
					Description = asset.Description,
					Quantity = asset.Quantity,
					OwnerId = asset.OwnerId
				})
				.ToListAsync();

			return Ok(selectedAssets);
		}
	/// <summary>
	/// get the item by id, if the user is not authorized, return unauthorized with a message
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			int? userID = GetCurrentUserID();

			if (userID == null)
			{// id have wrong format or session is invalid, then sign out and return unauthorized with a message
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
//get all id staff
			Asset? asset = await userContext.Assets.FindAsync(id);

			if (asset == null)
			{
				return NotFound(new { message = "Item not found." });
			}
// meas that you shoukd't have 
			if (asset.OwnerId != userID.Value)
			{
				return Forbid();
			}

			return Ok(new AssetResponseDTO
			{
				AssetId = asset.AssetId,
				AssetName = asset.Name,
				Description = asset.Description,
				Quantity = asset.Quantity
			});
		}
		/// <summary>
		/// create an asset
		/// </summary>
		/// <param name="add"></param>
		/// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetRequestDTO add)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			int? userID = GetCurrentUserID();
			//wrong user
			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
			// found userid 
			User? currentUser = await userContext.Users.FindAsync(userID.Value);

			if (currentUser == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "User not found." });
			}
			// quantity should be non-negative
			if (add.Quantity < 0)
			{
				return BadRequest(new { message = "Item quantity cannot be less than 0." });
			}
			// new asset create
			Asset newAsset = new Asset()
			{
				Name = add.AssetName,
				Description = string.IsNullOrEmpty(add.Description) ? string.Empty : add.Description,
				Quantity = add.Quantity,
				Owner = currentUser,
				OwnerId = currentUser.UserId
			};
///save to database
			await userContext.Assets.AddAsync(newAsset);
			await userContext.SaveChangesAsync();

			return Ok(new
			{
				message = "Item added successfully.",
				assetID = newAsset.AssetId
			});
		}
	/// <summary>
	/// use put to update it
	/// </summary>
	/// <param name="id"></param>
	/// <param name="update"></param>
	/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] UpdateAssetRequestDTO update)
		{
			//check state and user
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			int? userID = GetCurrentUserID();

			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
			//id not match
			if (id != update.ItemID)
			{
				return BadRequest(new { message = "Item ID mismatch." });
			}

			Asset? userAsset = await userContext.Assets.FindAsync(update.ItemID);
			// check if the asset exist, if not exist, return not found with a message
			if (userAsset == null)
			{
				return NotFound(new { message = "Item not found." });
			}
			// check if the asset belong to current user, if not belong, return forbid
			if (userAsset.OwnerId != userID.Value)
			{
				return Forbid();
			}
			// quantity should be non-negative
			if (update.Quantity < 0)
			{
				return BadRequest(new { message = "Quantity cannot be negative." });
			}
			// update the asset information and save to database
			userAsset.Name = update.ItemName;
			userAsset.Description = string.IsNullOrEmpty(update.Description) ? string.Empty : update.Description;
			userAsset.Quantity = update.Quantity;

			await userContext.SaveChangesAsync();

			return Ok(new
			{
				message = "Item updated successfully."
			});
		}
		/// <summary>
		/// delete the asset, if the user is not authorized, return unauthorized with a message, if the asset is not found, return not found with a message, if the asset does not belong to current user, return forbid, if delete successfully, return ok with a message
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			//check user
			int? userID = GetCurrentUserID();

			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}
			// check if the asset exist, if not exist, return not found with a message
			Asset? userAsset = await userContext.Assets.FindAsync(id);

			if (userAsset == null)
			{
				return NotFound(new { message = "Item not found." });
			}

			if (userAsset.OwnerId != userID.Value)
			{
				return Forbid();
			}
			//rremove the asset from database
			userContext.Assets.Remove(userAsset);
			await userContext.SaveChangesAsync();

			return Ok( new
			{
				message = "Item deleted successfully."
			});
		}
		/// <summary>
		/// get current user id from claim, if the claim is invalid, return null
		/// </summary>
		/// <returns></returns>
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
