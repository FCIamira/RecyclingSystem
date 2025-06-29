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
    public class WarehouseInventory : BaseModel<int>
    {
        [Required]
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        [Required]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Quantity must be non-negative.")]
        public double Quantity { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        //Navigation Property
        public Warehouse? Warehouse { get; set; }
        //public Material? Material { get; set; }
    }
}
