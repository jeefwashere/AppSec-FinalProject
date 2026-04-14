/*
 * FILE : AssetResponseDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores asset response data returned to the requesting user.
 */
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
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
