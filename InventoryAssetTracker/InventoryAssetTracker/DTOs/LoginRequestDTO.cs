using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{/// for login request, it has username and password
	public class LoginRequestDTO
	{
		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		public string? ReturnUrl { get; set; }
	}
}
