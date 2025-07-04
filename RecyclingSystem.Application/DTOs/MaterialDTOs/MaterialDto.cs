using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.MaterialDTOs
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PointsPerUnit { get; set; }
        public string UnitType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
