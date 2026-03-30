using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? DateOfDeath { get; set; }
        public string? Nationality { get; set; }
        public int? TotalBooks { get; set; } = 0;
        public byte[]? Image { get; set; }
        #region Relationship
        public ICollection<Book> Books { get; set; }
        #endregion
    }

}
