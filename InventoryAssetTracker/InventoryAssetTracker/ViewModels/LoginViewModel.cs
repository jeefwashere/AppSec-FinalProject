/*
 * FILE : LoginViewModel.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the login view model used for account pages.
 */
﻿// Referenecs: MVC Pattern https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-10.0&tabs=visual-studio 
using System.ComponentModel.DataAnnotations;

namespace InventoryAssetTracker.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		public string Username { get; set; } = string.Empty;
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;
		public string? ReturnUrl { get; set; } = null;
	}
}
