using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.EmployeeInfoDTOs
{
    public class EmployeeAvailabilityDto
    {
        public List<EmployeeDto> StrictlyAvailableEmployees { get; set; } = new();
        public List<SuggestionDto> SuggestedEmployees { get; set; } = new();  // ✅ جديد

        public SuggestionDto BestSuggestion { get; set; }
    }
}
