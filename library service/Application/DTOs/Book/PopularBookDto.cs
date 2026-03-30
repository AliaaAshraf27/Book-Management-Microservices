using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Book
{
    public class PopularBookDto
    {
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string? image { get; set; }
        public int BorrowingCount { get; set; }
    }
}
