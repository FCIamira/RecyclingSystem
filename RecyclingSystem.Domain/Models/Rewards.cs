using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Domain.Common;
using static System.Net.Mime.MediaTypeNames;
namespace RecyclingSystem.Domain.Models
{
    public class Rewards:BaseModel<int>
    {
      
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int PointsRequired { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<RewardRedemptions> ?RewardRedemptions { get; set; }
    }
}
