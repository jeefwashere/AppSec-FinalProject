// References: DTO https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
namespace InventoryAssetTracker.DTOs
{
	public class AdminLogResponseDTO
	{
		public string Action { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
	}
}