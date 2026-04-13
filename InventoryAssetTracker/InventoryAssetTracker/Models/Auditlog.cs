using InventoryAssetTracker.Models.YourProjectName.Models;

namespace InventoryAssetTracker.Models
{
    public class Auditlog
    {
        public int AuditLogId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
