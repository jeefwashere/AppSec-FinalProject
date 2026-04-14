/*
* FILE : AdminAssetResponseDTO.cs
* PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
* DESCRIPTION : Stores admin asset response data sent back to clients.
*/
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
namespace InventoryAssetTracker.DTOs
{
    public class AdminAssetResponseDTO
    {
        public int AssetId { get; set; }

        public string AssetName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public string Owner { get; set; } = string.Empty;
    }
}
