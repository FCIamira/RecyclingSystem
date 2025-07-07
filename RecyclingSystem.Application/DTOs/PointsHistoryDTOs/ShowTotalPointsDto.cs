using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PointsHistoryDTOs
{
    public class ShowTotalPointsDto
    {
        public double TotalPointsRedeemed { get; set; } 
        public double TotalPointsEarned { get; set; }
        public double TotalPointsAwarded { get; set; }

    }
}
