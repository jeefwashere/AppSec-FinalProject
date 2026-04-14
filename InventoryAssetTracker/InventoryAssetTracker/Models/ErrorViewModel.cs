/*
 * FILE : ErrorViewModel.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the error view model used by error pages.
 */
namespace InventoryAssetTracker.Models
{
	public class ErrorViewModel
	{
		public string? RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}
