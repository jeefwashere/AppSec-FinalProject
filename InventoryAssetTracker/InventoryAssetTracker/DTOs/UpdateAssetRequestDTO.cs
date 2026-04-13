using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{
    public class UpdateAssetRequestDTO
    {
        [Required]
        public int ItemID { get; set; }

        [Required]
        public string ItemName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

      
    }
}
