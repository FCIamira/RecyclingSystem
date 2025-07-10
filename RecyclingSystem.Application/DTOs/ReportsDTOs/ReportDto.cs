using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.ReportsDTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public ReportType Type { get; set; }
        public int? PickupRequestId { get; set; }
        public int? WarehouseId { get; set; }
        public string Description { get; set; }
        public ReportStatus Status { get; set; }
        public string? Response { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
