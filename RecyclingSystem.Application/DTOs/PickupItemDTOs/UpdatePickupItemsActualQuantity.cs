using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupItemDTOs
{
    public class UpdatePickupItemsActualQuantity
    {
        public int MaterialId { get; set; }
        public int ActualQuantity { get; set; } // The actual quantity collected by the employee
    }
}
