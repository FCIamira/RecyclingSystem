using RecyclingSystem.Application.DTOs.MaterialDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupRequestDTOs
{
    public class GetSuccessfullwithCancelDto
    {
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public List<GetMaterialWithQuantityDto>? MaterialWithQuantity { get; set; }
    }
}
