using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public byte[]? Image { get; set; }  
        public decimal? Price { get; set; }
        public bool IsAvailableForBorrowing { get; set; }
        public int TotalCopies { get; set; } = 0;
        public int AvailableCopies { get; set; } = 0;
        public int? PublishedYear { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        #region Relationship
        public Guid AuthorId { get; set; }
        public Author? Author { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Borrowing> Borrowings { get; set; } 
        public ICollection<Review> Reviews { get; set; }
        #endregion
    }

}
