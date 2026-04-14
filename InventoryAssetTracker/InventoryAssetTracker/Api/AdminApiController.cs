/*
 * FILE : AdminApiController.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles admin requests for managing users, assets, and system logs.
 */
using InventoryAssetTracker.Data;
using InventoryAssetTracker.DTOs;
using InventoryAssetTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
//            Authorization and Authentication https://www.w3tutorials.net/blog/asp-net-core-simplest-possible-forms-authentication/
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

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] AdminUpdateUserDTO update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(adminIdClaim, out int adminId))
            {
                return Unauthorized(new { message = "Invalid session." });
            }

            User? user = await userContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            bool usernameTaken = await userContext.Users
                .AnyAsync(u => u.UserId != id && u.Username == update.Username);

            if (usernameTaken)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            bool emailTaken = await userContext.Users
                .AnyAsync(u => u.UserId != id && u.Email == update.Email);

            if (emailTaken)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            user.Username = update.Username;
            user.Email = update.Email;

            if (adminId == id && user.Role != update.Role)
            {
                return BadRequest(new { message = "You cannot change your own role." });
            }

            user.Role = update.Role;

            await userContext.SaveChangesAsync();

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string? adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(adminIdClaim, out int adminId))
            {
                return Unauthorized(new { message = "Invalid session." });
            }

            if (adminId == id)
            {
                return BadRequest(new { message = "You cannot delete your own account." });
            }

            User? user = await userContext.Users
                .Include(u => u.Assets)
                .Include(u => u.UploadRecords)
                .Include(u => u.UserLogs)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            if (user.Role == "Admin")
            {
                int adminCount = await userContext.Users.CountAsync(u => u.Role == "Admin");
                if (adminCount <= 1)
                {
                    return BadRequest(new { message = "Cannot delete the last admin." });
                }
            }

            userContext.Assets.RemoveRange(user.Assets);
            userContext.UploadRecords.RemoveRange(user.UploadRecords);
            userContext.Logs.RemoveRange(user.UserLogs);
            userContext.Users.Remove(user);

            await userContext.SaveChangesAsync();


            return Ok(new { message = "User deleted successfully." });
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

        [HttpPut("assets/{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] AdminUpdateAssetDTO update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Asset? asset = await userContext.Assets.FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null)
            {
                return NotFound(new { message = "Asset not found." });
            }

            asset.Name = update.AssetName;
            asset.Description = update.Description ?? string.Empty;
            asset.Quantity = update.Quantity;

            await userContext.SaveChangesAsync();

            return Ok(new { message = "Asset updated successfully." });
        }

        [HttpDelete("assets/{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            Asset? asset = await userContext.Assets.FirstOrDefaultAsync(a => a.AssetId == id);

            if (asset == null)
            {
                return NotFound(new { message = "Asset not found." });
            }

            userContext.Assets.Remove(asset);
            await userContext.SaveChangesAsync();

            return Ok(new { message = "Asset deleted successfully." });
        }

        [HttpGet("logs")]
        public async Task<ActionResult<List<AdminLogResponseDTO>>> GetLogs()
        {
            List<AdminLogResponseDTO> logs = await userContext.Logs
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new AdminLogResponseDTO
                {
                    Action = l.Action,
                    Details = l.Details,
                    Username = l.Username,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}
