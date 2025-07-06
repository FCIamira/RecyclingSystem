using Microsoft.Extensions.Options;
using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // ✅ Required for data annotations
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RecyclingSystem.Domain.Models
{
    public class Report : BaseModel<int>
    {
        [Required(ErrorMessage = "EmployeeId is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Report Type is required.")]
        public ReportType Type { get; set; }
        [Required]
        public int PickupRequestId { get; set; }

        public int? WarehouseId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Pending;


        [StringLength(1000, ErrorMessage = "Response cannot exceed 1000 characters.")]
        public string? Response { get; set; }

      
        //Navigation Properties
      

        [ForeignKey("EmployeeId")]
        public virtual ApplicationUser Employee { get; set; }

        [ForeignKey("PickupRequestId")]
        public virtual PickupRequest? PickupRequest { get; set; }

        [ForeignKey("WarehouseId")]
        public virtual Warehouse? Warehouse { get; set; }
    }
}
