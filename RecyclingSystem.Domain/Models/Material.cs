using RecyclingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace RecyclingSystem.Domain.Models
{
    public class Material : BaseModel<int>
    {
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public int PointsPerUnit { get; set; }

        [MaxLength(50)]
        public string UnitType { get; set; }

        public bool IsActive { get; set; }
        public ICollection<FactoryOrders>? FactoryOrders { get; set; }
        public ICollection<PickupItem>? PickupItems { get; set; }
        public ICollection<WarehouseInventory>? WarehouseInventories { get; set; }
    }
}
