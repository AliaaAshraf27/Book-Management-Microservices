using Application.DTOs.Borrowing;
using Application.Enums;
using Application.Event;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class BorrowingProfile : Profile
    {
        public BorrowingProfile()
        {
            CreateMap<BorrowBookDto, Borrowing>()
                 .ForMember(d => d.Id,opt => opt.MapFrom(_ => Guid.NewGuid()))
                 .ForMember(d => d.Status,opt => opt.MapFrom(_ => BorrowingStatus.Pending))
                 .ForMember(d => d.RequestDate,opt => opt.MapFrom(_ => DateTime.UtcNow))
                 .ForMember(d => d.UserId, opt => opt.Ignore());

            CreateMap<Borrowing, BorrowingDto>()
                .ForMember(d => d.UserName , opt => opt.MapFrom(u => u.User.Name))
                .ForMember(d => d.BookTitle, opt => opt.MapFrom(src => src.Book.Title));

            CreateMap<Borrowing, UserBorrowingDto>()
                .ForMember(d => d.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
            //CreateMap<Borrowing, BorrowRequestedEvent>()
            //.ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            //.ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            //.ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
            //.ForMember(dest => dest.Action, opt => opt.Ignore());// القيمة في الكود

        }
    }
}
