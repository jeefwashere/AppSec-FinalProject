using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryAssetTracker.Api
{
	[Route("api/admin")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class AdminApiController : ControllerBase
	{
		private readonly UserContext userContext;

		public AdminApiController(UserContext userContext)
		{
			this.userContext = userContext;
		}

		[HttpGet("users")]
		public async Task<ActionResult<List<AdminUserResponseDTO>>> GetUsers()
		{
			List<AdminUserResponseDTO> users = await userContext.Users
				.OrderBy(u => u.UserId)
				.Select(u => new AdminUserResponseDTO
				{
					UserId = u.UserId,
					Username = u.Username,
					Email = u.Email,
					Role = u.Role
				})
				.ToListAsync();

			return Ok(users);
		}

		[HttpGet("assets")]
		public async Task<ActionResult<List<AdminAssetResponseDTO>>> GetAssets()
		{
			List<AdminAssetResponseDTO> assets = await userContext.Assets
				.Include(a => a.Owner)
				.OrderBy(a => a.AssetId)
				.Select(a => new AdminAssetResponseDTO
				{
					AssetId = a.AssetId,
					AssetName = a.Name,
					Description = a.Description,
					Quantity = a.Quantity,
					Owner = a.Owner != null ? a.Owner.Username : "Unknown"
				})
				.ToListAsync();

			return Ok(assets);
		}

		[HttpGet("logs")]
		public async Task<ActionResult<List<AdminLogResponseDTO>>> GetLogs()
		{
			List<AdminLogResponseDTO> logs = await userContext.AuditLogs
				.Include(l => l.User)
				.OrderByDescending(l => l.CreatedAt)
				.Select(l => new AdminLogResponseDTO
				{
					Action = l.Action,
					Details = l.Details,
					Username = l.User != null ? l.User.Username : "Unknown",
					CreatedAt = l.CreatedAt
				})
				.ToListAsync();

			return Ok(logs);
		}
	}
}