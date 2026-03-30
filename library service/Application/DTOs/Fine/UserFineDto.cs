using Application.DTOs.Borrowing;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Fine
{
    public class UserFineDto
    {
        public Guid BorrowingId { get; set; }
        public UserBorrowingDto? BorrowingDto { get; set; }
        public int? DaysOverdue { get; set; }
        public decimal? DailyFineRate { get; set; }
        public decimal? FineAmount { get; set; }
        public PaidStatus PaidStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
