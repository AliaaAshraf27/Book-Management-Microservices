using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public byte[]? Image { get; set; }
        // بيانات عضوية المكتبة
        public DateTime MembershipDate { get; set; } = DateTime.Now;
        public DateTime MembershipExpiryDate { get; set; } = DateTime.Now.AddMonths(3);
        public int MaxBorrowLimit { get; set; } = 5;

        public bool IsSuspended { get; set; }
        public string? SuspensionReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

}
