using AutoMapper;
using RecyclingSystem.Application.DTOs.WharehosingInventoryDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class WarehouseInventoryProfile:Profile
    {

        public WarehouseInventoryProfile()
        {
            CreateMap<WarehouseInventory, WarehouseInventoryDataDTO>()
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Warehouse.Location))
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => src.LastUpdated));
        }
    }
}
