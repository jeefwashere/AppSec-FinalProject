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
using InventoryAssetTracker.Models.YourProjectName.Models;
using InventoryAssetTracker.ViewModels;

namespace InventoryAssetTracker.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemApiController : ControllerBase
    {
        private readonly UserContext userContext;

        public ItemApiController(UserContext userContext)
        {
            this.userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyItems()
        {
            int? userID = GetCurrentUserID();

            if (userID == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized(new { message = "Invalid session." });
            }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
		{
            int? userID = GetCurrentUserID();

			if (userID == null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "Invalid session." });
			}

            Asset? asset = await userContext.Assets.FindAsync(id);

            if (asset == null)
            {
                return NotFound(new { message = "Item not found." });
            }

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddAssetViewModel add)
        {
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

            User? currentUser = await userContext.Users.FindAsync(userID.Value);

            if (currentUser == null)
            {
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return Unauthorized(new { message = "User not found." });
			}

            if (add.Quantity < 0)
            {
                return BadRequest(new { message = "Item quantity cannot be less than 0." });
            }

            Asset newAsset = new Asset()
            {
                Name = add.AssetName,
                Description = string.IsNullOrEmpty(add.Description) ? string.Empty : add.Description,
                Quantity = add.Quantity,
                Owner = currentUser,
                OwnerId = currentUser.UserId
            };

            await userContext.Assets.AddAsync(newAsset);
            await userContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Item added successfully.",
                assetID = newAsset.AssetId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
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
