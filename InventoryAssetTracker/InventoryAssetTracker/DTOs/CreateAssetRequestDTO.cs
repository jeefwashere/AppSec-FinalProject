/*
 * FILE : CreateAssetRequestDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores new asset request data submitted by the user.
 */
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
using System.ComponentModel.DataAnnotations;

// this is for create asset request, it has asset name, description and quantity, the asset name is required, the quantity should be non-negative
namespace InventoryAssetTracker.DTOs
{
    public class CreateAssetRequestDTO
    {
        [Required]
        public string AssetName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }
    }
}
