
namespace InventoryAssetTracker.Models

{
	public class User
	{
		public int UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = "User";
		public string Username { get; set; } = string.Empty;
		public string? ProfilePhotoPath { get; set; }
		public List<Asset> Assets { get; set; } = new List<Asset>();
		public List<Auditlog> AuditLogs { get; set; } = new List<Auditlog>();
	}

}
