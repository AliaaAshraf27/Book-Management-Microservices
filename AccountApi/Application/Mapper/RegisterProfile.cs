using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Auth.DTOs;
using AutoMapper;
using Domain.Entities;
namespace Application.Mapper
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterModel, User>();

        }
    }
}
