using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public decimal? Price { get; set; }
        public int? PublishedYear { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CategoryId { get; set; }
        public int TotalCopies { get; set; }

    }


}
