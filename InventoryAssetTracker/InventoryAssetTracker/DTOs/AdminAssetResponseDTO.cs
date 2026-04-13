namespace InventoryAssetTracker.DTOs
{
	public class AdminAssetResponseDTO
	{
		public int AssetId { get; set; }
		public string AssetName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public string Owner { get; set; } = string.Empty;
	}
}