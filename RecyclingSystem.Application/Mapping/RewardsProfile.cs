using AutoMapper;
using RecyclingSystem.Application.DTOs.RewardsDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class RewardsProfile:Profile
    {
        public RewardsProfile() {
        
        CreateMap<Rewards,RewardDTO>(); 

            CreateMap<Rewards,CreateRewardDTO>().ReverseMap();
                }
    }
}
