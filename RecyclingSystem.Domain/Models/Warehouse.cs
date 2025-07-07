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
    public class Warehouse : BaseModel<int>
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

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }

        //Navigation Property
        public ApplicationUser? Manager { get; set; }
        public virtual ICollection<WarehouseInventory>? Inventory { get; set; }
        public virtual ICollection<FactoryOrders>? FactoryOrders { get; set; }
        public virtual ICollection<EmployeeWarehouseHistory>? Employees { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
