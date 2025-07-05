using RecyclingSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Models
{
    public class UserGift : BaseModel<int>
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        public int GiftCount { get; set; } 

        public int PointsPerGift { get; set; }  

        public int TotalGiftPoints => GiftCount * PointsPerGift;  

        public DateTime LastUpdated { get; set; }
    }

}
