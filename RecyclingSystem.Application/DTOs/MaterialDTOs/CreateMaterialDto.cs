using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.MaterialDTOs
{
    public class CreateMaterialDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PointsPerUnit { get; set; }
        public string UnitType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
