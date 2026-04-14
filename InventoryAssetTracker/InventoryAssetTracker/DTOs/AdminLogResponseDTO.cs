/*
 * FILE : AdminLogResponseDTO.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Stores admin log response data displayed in dashboard pages.
 */
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
