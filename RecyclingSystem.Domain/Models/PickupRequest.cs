using RecyclingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingSystem.Domain.Models
    {
        public class PickupRequest : BaseModel<int>
        {
            [ForeignKey("User")]
            public int CustomerId { get; set; }

            public DateTime RequestedDate { get; set; }

            public DateTime? ScheduledDate { get; set; }

            public string Location { get; set; }

            public string Note { get; set; }

            public string Status { get; set; }

            public int? EmployeeId { get; set; }

            public DateTime? DateCollected { get; set; }

            public int TotalPointsGiven { get; set; }


            //Navigation Property
            public virtual Users? User { get; set; }
            public ICollection<PickupItem>? PickupItems { get; set; }
        }
    }