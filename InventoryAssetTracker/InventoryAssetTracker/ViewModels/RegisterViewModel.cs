/*
 * FILE : RegisterViewModel.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the register view model used for account pages.
 */
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
