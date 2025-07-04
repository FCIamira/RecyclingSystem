using AutoMapper;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.DTOs.PickupRequestDTOs
{
    public class GetAllRequestDto
    {
        public DateTime RequestedDate { get; set; }
        public string Status { get; set; }
        public List<GetMaterialWithQuantityDto>? MaterialWithQuantity { get; set; }
    }
}
