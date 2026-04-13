namespace InventoryAssetTracker.DTOs
{
    public class AuditLogResponseDTO

    {
        public int AuditLogId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public string? Username { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
