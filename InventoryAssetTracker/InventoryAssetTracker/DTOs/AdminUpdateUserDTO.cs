/*
 * FILE : AdminUpdateUserDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores admin user update data submitted from management forms.
 */
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{
    public class AdminUpdateUserDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
