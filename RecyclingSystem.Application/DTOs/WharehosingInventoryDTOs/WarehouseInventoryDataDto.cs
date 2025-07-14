using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs
{
    public class WarehouseInventoryDataDTO
    {
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public string MaterialName { get; set; }
        public double Quantity { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
