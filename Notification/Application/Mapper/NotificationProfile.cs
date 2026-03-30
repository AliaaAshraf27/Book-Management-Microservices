using Application.Services.Notifications.DTOS;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() 
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}
