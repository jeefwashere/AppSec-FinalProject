namespace InventoryAssetTracker.Models
{
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
