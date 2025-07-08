using RecyclingSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyclingSystem.Domain.Models
{
    public class PickupItem : BaseModel<int>
    {
        [ForeignKey("PickupRequest")]
        public int PickupRequestId { get; set; }
        [ForeignKey("Material")]
        public int MaterialId { get; set; }

        public int PlannedQuantity { get; set; }
        public int ActualQuantity { get; set; } = 0;
        public virtual PickupRequest? PickupRequest { get; set; }

        public virtual Material? Material { get; set; }
    }
}
