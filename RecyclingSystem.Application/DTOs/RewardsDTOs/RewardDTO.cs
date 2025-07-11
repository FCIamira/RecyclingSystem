using Microsoft.AspNetCore.Http;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.RewardsDTOs
{
    public class RewardDTO
    {
        public int Id { get;set; }  
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int PointsRequired { get; set; }
        public int StockQuantity { get; set; }

    }
}
