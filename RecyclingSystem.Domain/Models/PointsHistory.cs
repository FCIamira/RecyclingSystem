﻿using RecyclingSystem.Domain.Common;
using RecyclingSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Models
{
    public class PointsHistory : BaseModel<int>
    {
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public PointsHistoryTypes Type { get; set; }

        [Required]
        [Range(-10000, 10000, ErrorMessage = "Points must be a reasonable change")]
        public int PointsChanged { get; set; }

        [Required]
        [StringLength(255)]
        public string Reason { get; set; }

        public DateTime Datetime { get; set; } = DateTime.UtcNow;
    }
}
