// References: DTO https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
﻿/*
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
