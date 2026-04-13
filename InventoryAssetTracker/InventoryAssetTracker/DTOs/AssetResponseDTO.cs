namespace InventoryAssetTracker.DTOs
{
	public class AssetResponseDTO
	{
		public int AssetId { get; set; }
		public string AssetName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public int OwnerId { get; set; }
	}
}
