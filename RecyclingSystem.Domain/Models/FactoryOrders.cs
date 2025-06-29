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
    public class FactoryOrders : BaseModel<int>
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Quantity must be non-negative.")]
        public double Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public double PricePerUnit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total price must be non-negative.")]
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey("Warehouse")]
        public int WarehouseId { get; set; }

        [Required]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        //Navigation Property
        public Warehouse? Warehouse { get; set; }
        public Material? Material { get; set; }
    }
}
