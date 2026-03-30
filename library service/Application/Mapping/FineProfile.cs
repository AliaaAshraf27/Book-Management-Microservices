using Application.DTOs.Fine;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class FineProfile : Profile
    {
        public FineProfile() 
        {
            CreateMap<Fine, FineDto>()
               .ForMember(dest => dest.UserName,opt => opt.MapFrom(src => src.User.Name))
               .ForMember(dest => dest.BorrowingDto,opt => opt.MapFrom(src => src.Borrowing));

            CreateMap<Fine, UserFineDto>()
               .ForMember(dest => dest.BorrowingDto, opt => opt.MapFrom(src => src.Borrowing));
            CreateMap<CreateFineDto,Fine>();

        }
    }
}
