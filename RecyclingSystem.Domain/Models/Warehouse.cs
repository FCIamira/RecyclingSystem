using RecyclingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Models
{
    internal class Warehouse : BaseModel<int>
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string Name { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Location must be at least 5 characters.")]
        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Capacity must be non-negative.")]
        public double? CapacityKg { get; set; }

        [ForeignKey("User")]
        public int ManagerId { get; set; }

        //Navigation Property
        //public Users? User { get; set; }
        public ICollection<WarehouseInventory>? Inventory { get; set; }
        public ICollection<FactoryOrders>? FactoryOrders { get; set; }
    }
}
