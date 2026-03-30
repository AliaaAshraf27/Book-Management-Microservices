using Azure.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Book
{
    public class BooksDTO
    {
        public string Title { get; set; }
        public string? image { get; set; }
        public string CategoryName { get; set; }
        public decimal? Price { get; set; }
    }

}
