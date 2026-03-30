using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Fine
{
    public class CreateFineDto
    {
        public string UserId { get; set; }
        public Guid BorrowingId { get; set; }
        public int DaysOverdue { get; set; }
        public decimal DailyFineRate { get; set; }
        public decimal FineAmount { get; set; }
    }
}
