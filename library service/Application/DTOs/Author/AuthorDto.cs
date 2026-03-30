using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Author
{
    public class AuthorDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? DateOfDeath { get; set; }
        public string? Nationality { get; set; }
        public string? Image { get; set; }
    }
}
