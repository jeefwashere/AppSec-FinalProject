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
        public IActionResult Create()
        {
            return Ok();
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
