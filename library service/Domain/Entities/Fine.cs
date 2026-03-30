using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Fine
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid BorrowingId { get; set; }
        public Borrowing? Borrowing { get; set; }
        public int? DaysOverdue { get; set; } // عدد الايام 
        public decimal? DailyFineRate { get; set; } = .5m; // نسبة الغرامة 
        public decimal? FineAmount { get; set; } = 0; // الاجمالي
        public PaidStatus PaidStatus { get; set; } = PaidStatus.Unpaid;      
        public DateTime? PaymentDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
