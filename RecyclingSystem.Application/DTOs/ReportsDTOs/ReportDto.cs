using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.ReportsDTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public ReportType Type { get; set; }
        [ForeignKey("PickupRequest")]
        public int? PickupRequestId { get; set; }
        public string? CustomerName { get; set; }
        public string? RequestAddress { get; set; }
        public string? Description { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser? Employee {  get; set; }
        public PickupRequest? PickupRequest { get; set; }
    }
}
