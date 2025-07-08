using RecyclingSystem.Application.DTOs.PickupItemDTOs;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupRequestDTOs
{
    public class GetEmployeePickupRequestDto
    {
        public int Id { get; set; }
        public UserInfoDto Customer { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public string Address { get; set; } = string.Empty;
        public string LocationLat { get; set; }
        public string LocationLng { get; set; }

        public string Note { get; set; }

        public PickupStatus Status { get; set; }

        public DateTime? DateCollected { get; set; }

        public int? TotalPointsGiven { get; set; }

        public ICollection<PickupItemDto>? PickupItems { get; set; }
    }
}
