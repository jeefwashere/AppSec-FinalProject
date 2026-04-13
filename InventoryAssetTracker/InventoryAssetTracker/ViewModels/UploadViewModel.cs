using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
    public class UploadViewModel
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}
