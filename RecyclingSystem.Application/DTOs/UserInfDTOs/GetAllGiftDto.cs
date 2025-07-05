using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.UserInfDTOs
{
    public class GetAllGiftDto

    { 
            public int GiftCount { get; set; }

            public int PointsPerGift { get; set; }

            public int TotalGiftPoints { get; set; }

            public DateTime LastUpdated { get; set; }
        }

    }