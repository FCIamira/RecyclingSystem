using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Enums;
namespace RecyclingSystem.Domain.Models
{
    public class RewardRedemptions:BaseModel<int>
    {
        [ForeignKey(nameof(User))]
       public int UserId {   get; set;}
        [ForeignKey(nameof(Reward))]
       public int  RewardId {  get; set;}
        public DateTime DateRedeemed {get;set;}
        public Status RedemptionStatus { get; set;}
        public int TotalPoints { get; set;}
        public int Quantity { get; set;}
        public virtual Rewards? Reward { get; set;}
        public virtual ApplicationUser? User { get; set; }//not completed
    }
}
