/*
 * FILE : RegisterRequestDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores registration request data submitted by new users.
 */
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{
    public class RegisterRequestDTO
    {
		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;
	}
}
