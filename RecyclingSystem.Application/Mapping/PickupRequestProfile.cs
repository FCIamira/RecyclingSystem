using AutoMapper;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Application.DTOs.PickupRequestDTOs;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Application.DTOs.MaterialDTOs;
namespace RecyclingSystem.Application.Mapping
{
    public class PickupRequestProfile:Profile
    {
        public PickupRequestProfile()
        {
            CreateMap< PickupRequest, GetAllRequestDto>().ForMember(dest=>dest.Status,
                opt=>opt.MapFrom(src=>src.Status.ToString()))
                .ForMember(dest=>dest.MaterialWithQuantity,opt=>opt.MapFrom(src=>src.PickupItems !=null
                ?src.PickupItems.Where(p=>p.Material !=null)
                .GroupBy(p=>p.Material.Name)
                .Select(g=>new GetMaterialWithQuantityDto
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity)

                }).ToList(): new List<GetMaterialWithQuantityDto>())
                )
                .ReverseMap();
        }
    }
}
