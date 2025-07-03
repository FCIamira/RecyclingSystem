using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecyclingSystem.Application.DTOs.PickupRequestDTOs
{
    public class CreatePickupRequestDto
    {
        public string Address { get; set; } = string.Empty;
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime PreferredDate { get; set; } = DateTime.Now.AddDays(1);
        public string Note { get; set; } = string.Empty;
        public List<CreatePickupItemDto> PickupItems { get; set; } = new List<CreatePickupItemDto>();
    }
}
