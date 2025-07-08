using AutoMapper;
using RecyclingSystem.Application.DTOs.PointsHistoryDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class PointsHistoryProfile : Profile
    {
        public PointsHistoryProfile()
        {
            CreateMap<PointsHistory, ShowPointsHistoryDto>();
        }
    }
}
