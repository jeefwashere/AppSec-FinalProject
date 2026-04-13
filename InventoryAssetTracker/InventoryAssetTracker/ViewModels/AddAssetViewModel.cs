using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
    public class AddAssetViewModel
    {
        [Required]
        public string AssetName { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

    }
}
