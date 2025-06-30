using Microsoft.AspNet.Identity;
using RecyclingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingSystem.Domain.Models
    {
    public class PickupRequest : BaseModel<int>
    {
        public int CustomerId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public string LocationLat { get; set; }
        public string LocationLng { get; set; }

        public string Note { get; set; }

        public string Status { get; set; }

        public DateTime? DateCollected { get; set; }

        public int TotalPointsGiven { get; set; }

        //[ForeignKey("CustomerId")]
        //public virtual Users? Customer { get; set; }

        //[ForeignKey("EmployeeId")]
        //public virtual Users? Employee { get; set; }

        public ICollection<PickupItem>? PickupItems { get; set; }
    }

}