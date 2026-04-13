
namespace InventoryAssetTracker.Models

{
    namespace YourProjectName.Models
    {
        public class User
        {
            public int UserId { get; set; }
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string Role { get; set; } = "User";
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;

            public List<Asset> Assets { get; set; } = new();
            public List<Auditlog> AuditLogs { get; set; } = new();
        }
    }
}
