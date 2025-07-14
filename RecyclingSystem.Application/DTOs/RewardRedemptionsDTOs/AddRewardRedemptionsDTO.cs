using Microsoft.AspNet.Identity;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.RewardRedemptionsDTOs
{
    public class AddRewardRedemptionsDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RewardId { get; set; }

        // public DateTime DateRedeemed { get; set; }
        // public Status RedemptionStatus { get; set; } = Status.Pending;
        public int Quantity { get; set; }
    }
}
