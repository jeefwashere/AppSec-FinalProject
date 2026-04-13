using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
    public class AddAssetViewModel
    {
        [Required]
        public string AssetName { get; set; }
        public string Description { get; set; }
        [Required]
        public string Quantity { get; set; }
    }
}
