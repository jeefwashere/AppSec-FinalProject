// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
    public class UpdateAssetViewModel
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


