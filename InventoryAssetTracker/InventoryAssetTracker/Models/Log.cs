/*
 * FILE : Log.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Defines the log model used for action logging records.
 */
namespace InventoryAssetTracker.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
