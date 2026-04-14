/*
 * FILE : AuthResponseDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores authentication response data for login and register requests.
 */
// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
namespace InventoryAssetTracker.DTOs
{
	/// <summary>
	/// response for login and register, it has success status, message and redirect url for refirect
	/// </summary>
	public class AuthResponseDTO
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public string? RedirectUrl { get; set; }
	}
}
