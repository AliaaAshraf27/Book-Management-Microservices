using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Review
{
    public class ReviewFilterDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? UserName { get; set; }
        public string? BookTitle { get; set; }
        public string? Text { get; set; }
        public int? Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
