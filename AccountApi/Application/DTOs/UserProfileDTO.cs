using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserProfileDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime MembershipDate { get; set; } 
        public DateTime MembershipExpiryDate { get; set; } 
    }
}
