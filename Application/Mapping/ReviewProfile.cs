using Application.DTOs.Review;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() 
        { 
          CreateMap<Review , ReviewsDto>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(u => u.User.Name))
                .ForMember(d => d.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); 
        }
    }
}
