using InventoryAssetTracker.Models;

namespace InventoryAssetTracker.Models
{
    /// <summary>
    /// asset model for database, it has asset information and relationship with other tables
    /// </summary>
    public class Asset
    {

        public int AssetId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public List<Upload> Uploads { get; set; } = new();

    }
}
