using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupItemDTOs
{
    public class PickupItemDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public int PlannedQuantity { get; set; }
        public int? ActualQuantity { get; set; }
    }
}
