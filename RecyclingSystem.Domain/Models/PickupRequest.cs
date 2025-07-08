using Microsoft.AspNet.Identity;
using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingSystem.Domain.Models
    {
    public class PickupRequest : BaseModel<int>
    {
        public int CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string LocationLat { get; set; }
        public string LocationLng { get; set; }
        public string Note { get; set; }
        public PickupStatus Status { get; set; }
        public DateTime? DateCollected { get; set; }
        public int? TotalPointsGiven { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser? Customer { get; set; }
        
        [ForeignKey("EmployeeId")]
        public virtual ApplicationUser? Employee { get; set; }
        public ICollection<PickupItem>? PickupItems { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }

}