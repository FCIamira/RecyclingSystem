using AutoMapper;
using RecyclingSystem.Application.DTOs.UserInfDTOs;
using RecyclingSystem.Application.Feature.UserInfo.Command;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserInfoDto>();
            CreateMap<UpdateUserCommand, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
