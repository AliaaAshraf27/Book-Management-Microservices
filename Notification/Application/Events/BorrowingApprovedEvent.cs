using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Events
{
    public record BorrowingApprovedEvent(string UserId, string BookTitle, DateTime DueDate, string Message);
}
