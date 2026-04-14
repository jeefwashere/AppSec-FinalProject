/*
 * FILE : LoginRequestDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores login request data entered by the user.
 */
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.DTOs
{/// for login request, it has username and password
	public class LoginRequestDTO
	{
		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		public string Password { get; set; } = string.Empty;

		public string? ReturnUrl { get; set; }
	}
}
