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
        // this for each is database table for data type
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Upload> UploadRecords { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

    }
}
