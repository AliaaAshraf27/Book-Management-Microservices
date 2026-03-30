using Application.DTOs.Book;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BooksDTO>()
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.Image != null && src.Image.Length > 0
                     ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Book, BookDetailsDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null &&
                src.Image.Length > 0 ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null))
                .ForMember(dest => dest.CategoryDTO, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.AuthorDTO, opt => opt.MapFrom(src => src.Author));

            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.IsAvailableForBorrowing, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.Image,
               opt => opt.MapFrom(src => src.Image != null ? ConvertIFormFileToBytes(src.Image) : null));

            CreateMap<UpdateBookDto, Book>()
                 .ForMember(dest => dest.Image, opt => opt.MapFrom((src, dest) =>
                   src.Image != null ? ConvertIFormFileToBytes(src.Image) : dest.Image))
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


        }
        private static byte[]? ConvertIFormFileToBytes(IFormFile? file)
            {
                if (file == null) return null;
                using var dataStream = new MemoryStream();
                file.CopyToAsync(dataStream); 
                return dataStream.ToArray();
            }


    }
}
