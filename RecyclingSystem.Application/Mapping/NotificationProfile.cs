using AutoMapper;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class NotificationProfile : Profile
    {
       public NotificationProfile()
        {
            CreateMap<Notification, GetAllNotificationDto>();
        }
    }
}
