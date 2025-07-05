using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupItemDTOs
{
    public class CreatePickupItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
    }
}
