/*
 * FILE : UpdateAssetViewModel.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the update asset view model used for item pages.
 */
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

