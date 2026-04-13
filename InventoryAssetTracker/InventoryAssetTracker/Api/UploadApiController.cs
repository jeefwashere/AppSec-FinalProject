using InventoryAssetTracker.Data;
using InventoryAssetTracker.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace InventoryAssetTracker.Api
{
    [Route("Upload")]
    [ApiController]
    [Authorize]
    public class UploadApiController : ControllerBase
    {
        private readonly UserContext userContext;
        private readonly IWebHostEnvironment environment;

        public UploadApiController(UserContext userContext, IWebHostEnvironment environment)
        {
            this.userContext = userContext;
            this.environment = environment;
        }

        [HttpPost("UploadProfilePhoto")]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
        {
            int? userID = GetCurrentUserID();

            // 🔥 CHANGE: removed forced sign-out (not needed)
            if (userID == null)
            {
                return Unauthorized(new { message = "Invalid session." });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "No file was uploaded."
                });
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    message = "Only JPG, JPEG, PNG, and WEBP Files are allowed."
                });
            }

            long maxFileSize = 2 * 1024 * 1024;
            if (file.Length > maxFileSize)
            {
                return BadRequest(new
                {
                    message = "File size must be 2 MB or less."
                });
            }

            User? user = await userContext.Users.FirstOrDefaultAsync(u => u.UserId == userID.Value);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found."
                });
            }

            string uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "profilephotos");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"user_{user.UserId}_{Guid.NewGuid()}{fileExtension}";
            string fullFilePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (FileStream stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (!string.IsNullOrWhiteSpace(user.ProfilePhotoPath))
            {
                string oldFileName = Path.GetFileName(user.ProfilePhotoPath);
                string oldFullPath = Path.Combine(uploadsFolder, oldFileName);

                if (System.IO.File.Exists(oldFullPath))
                {
                    System.IO.File.Delete(oldFullPath);
                }
            }

            user.ProfilePhotoPath = $"/uploads/profilephotos/{uniqueFileName}";
            await userContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Profile photo uploaded successfully.",
                photoPath = user.ProfilePhotoPath
            });
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