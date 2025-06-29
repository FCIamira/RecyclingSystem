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
    internal class RewardRedemptions:BaseModel<int>
    {
        [ForeignKey("User")]
       public int UserId {   get; set;}
        [ForeignKey("Rewards")]
       public int  RewardId {  get; set;}
        public DateTime DateRedeemed {get;set;}
        public Status Status { get; set;}

        public virtual Rewards Rewards { get; set;}
        //public virtual User User { get; set; }//not completed
    }
}
