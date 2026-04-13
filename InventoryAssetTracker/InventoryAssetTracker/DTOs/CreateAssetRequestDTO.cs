using System.ComponentModel.DataAnnotations;

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
