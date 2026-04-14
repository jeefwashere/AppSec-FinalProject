// References: https://medium.com/@mariorodrguezgalicia/what-is-a-dto-in-spring-boot-and-why-should-you-use-it-97651506e516 
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
