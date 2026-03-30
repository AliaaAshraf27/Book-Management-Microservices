using Application.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Borrowing
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }

        #region Relationship
        public BorrowingStatus Status { get; set; } = BorrowingStatus.Pending;
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Fine? Fine { get; set; }

        #endregion

    }

}
