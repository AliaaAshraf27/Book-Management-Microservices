using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Book
{
    public class UpdateBookDto
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Range(0, 100000)]
        public decimal? Price { get; set; }
        public int? PublishedYear { get; set; }
        public Guid? AuthorId { get; set; }
        public Guid? CategoryId { get; set; }
        public int? TotalCopies { get; set; }
        public bool? IsAvailableForBorrowing { get; set; }
    }
}
