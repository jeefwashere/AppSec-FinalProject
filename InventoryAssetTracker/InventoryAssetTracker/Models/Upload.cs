/*
 * FILE : Upload.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the upload model used for stored file records.
 */
namespace InventoryAssetTracker.Models
{
	/// <summary>
	/// upload model for database, it has upload information and relationship with other tables
	/// </summary>
    public class Upload
    {
		public int UploadId { get; set; }

		public string OriginalFileName { get; set; } = string.Empty;
		public string StoredFileName { get; set; } = string.Empty;
		public string FilePath { get; set; } = string.Empty;
		public string ContentType { get; set; } = string.Empty;
		public long FileSize { get; set; }

		public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

		public int UserId { get; set; }
		public User? User { get; set; }
	}
}
