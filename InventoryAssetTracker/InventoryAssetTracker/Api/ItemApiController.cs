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
            int? userId = GetCurrentUserID();

            if (userId == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Unauthorized(new { message = "Invalid session." });
            }

			List<AssetResponseDTO> selectedAssets = await userContext.Assets
	            .Where(asset => asset.OwnerId == userId.Value)
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
        public IActionResult GetById(int id)
        {
            return Ok();
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
