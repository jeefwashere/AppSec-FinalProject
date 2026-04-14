/*
 * FILE : UserContext.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Handles user-related data operations.
 */
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
        public DbSet<Log> Logs { get; set; }

    }
}
