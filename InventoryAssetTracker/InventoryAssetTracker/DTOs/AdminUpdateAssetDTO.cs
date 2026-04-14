/*
 * FILE : AdminUpdateAssetDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores admin asset update data submitted from edit forms.
 */
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{
    public class AdminUpdateAssetDTO
    {
        [Required]
        public string AssetName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
