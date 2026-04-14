/*
 * FILE : ProfileViewModel.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the profile view model used for account pages.
 */
namespace InventoryAssetTracker.ViewModels
{

    public class ProfileViewModel
    {
        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? ProfilePhotoPath { get; set; }
    }

}
