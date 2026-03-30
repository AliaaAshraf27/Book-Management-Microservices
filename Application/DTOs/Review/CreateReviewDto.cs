using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Review
{
    public class CreateReviewDto
    {
        public string? Text { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public Guid bookId { get; set; }
    }
}
