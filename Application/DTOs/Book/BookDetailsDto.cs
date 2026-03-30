using Application.DTOs.Author;
using Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Book
{
    public class BookDetailsDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal? Price { get; set; }
        public bool IsAvailableForBorrowing { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int? PublishedYear { get; set; }
        public AuthorDto AuthorDTO { get; set; }
        public CategoryDTO CategoryDTO { get; set; }


    }
}
