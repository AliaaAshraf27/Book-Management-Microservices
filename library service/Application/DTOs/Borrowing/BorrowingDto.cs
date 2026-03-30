using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Borrowing
{
    public class BorrowingDto
    {
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal LateFees { get; set; }
        public BorrowingStatus Status { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }


    }
}
