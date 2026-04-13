using Microsoft.EntityFrameworkCore;
using InventoryAssetTracker.Models;

namespace InventoryAssetTracker.Data

{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
    : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Upload> UploadRecords { get; set; }
        public DbSet<Auditlog> AuditLogs { get; set; }

    }
}
