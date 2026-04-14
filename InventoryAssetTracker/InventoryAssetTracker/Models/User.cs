/*
 * FILE : User.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the user model used for application accounts.
 */
﻿// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio
namespace InventoryAssetTracker.Models

{
	/// <summary>
	/// user model for database, it has user information and relationship with other tables
	/// </summary>
	public class User
	{
		public int UserId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public string Role { get; set; } = "User";
		public string Username { get; set; } = string.Empty;
		public string? ProfilePhotoPath { get; set; }
		public List<Asset> Assets { get; set; } = new List<Asset>();
		public List<Upload> UploadRecords { get; set; } = new List<Upload>();
		public List<Log> UserLogs { get; set; } = new List<Log>();
	}

}
