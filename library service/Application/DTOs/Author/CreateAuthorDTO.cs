using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Author
{
    public class CreateAuthorDTO
    {
        [MaxLength(50)]
        public string Name { get; set; }
        public int? TotalBooks { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? DateOfDeath { get; set; }

        [MaxLength(100)]
        public string? Nationality { get; set; }
        public IFormFile? Image { get; set; }
    }
}
