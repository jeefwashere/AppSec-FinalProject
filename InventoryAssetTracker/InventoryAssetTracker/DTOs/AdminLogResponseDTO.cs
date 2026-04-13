namespace InventoryAssetTracker.DTOs
{
	public class AdminLogResponseDTO
	{
		public string Action { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}