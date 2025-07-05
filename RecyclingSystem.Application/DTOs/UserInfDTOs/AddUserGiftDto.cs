using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.UserInfDTOs
{
    public class AddUserGiftDto
    {
        public int UserId { get; set; }
        public int GiftCount { get; set; }
        public int PointsPerGift { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
