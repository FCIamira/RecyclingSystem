using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.ReportsDTOs
{
    public class CreateReportDTO
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Report Type is required.")]
        public ReportType Type { get; set; }
        [Required]
        public int? PickupRequestId { get; set; }
        public string WarehouseName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

    }
}
