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
