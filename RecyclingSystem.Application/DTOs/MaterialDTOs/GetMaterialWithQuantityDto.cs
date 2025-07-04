using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.MaterialDTOs
{
    public class GetMaterialWithQuantityDto
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
