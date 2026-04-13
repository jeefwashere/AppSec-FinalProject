
namespace InventoryAssetTracker.Models

{
	/// <summary>
	/// user model for database, it has user information and relationship with other tables
	/// </summary>
	public class User
	{
		public int UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = "User";
		public string Username { get; set; } = string.Empty;
		public string? ProfilePhotoPath { get; set; }
		public List<Asset> Assets { get; set; } = new List<Asset>();
		public List<Upload> UploadRecords { get; set; } = new List<Upload>();
		public List<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
	}

}
