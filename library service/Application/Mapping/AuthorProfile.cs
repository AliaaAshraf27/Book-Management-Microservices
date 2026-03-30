using Application.DTOs.Author;
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
    public  class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null &&
                src.Image.Length > 0 ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null));

            CreateMap<Author, AuthorsDTO>()
                   .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null && src.Image.Length > 0
                        ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null));

            CreateMap<Author, AuthorDetailsDTO>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null && src.Image.Length> 0 ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null));

            CreateMap<CreateAuthorDTO, Author>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null ? ConvertIFormFileToBytes(src.Image) : null));

            CreateMap<UpdateAuthorDTO, Author>()
                 .ForMember(dest => dest.Image, opt => opt.MapFrom((src, dest) =>
                   src.Image != null ? ConvertIFormFileToBytes(src.Image) : dest.Image))
                 .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Author, GetAuthorWithBooks>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image != null &&
                  src.Image.Length > 0 ? $"data:image/png;base64,{Convert.ToBase64String(src.Image)}" : null))
                .ForMember(dest => dest.booksDTO,opt => opt.MapFrom(src => src.Books));


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
