using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Events
{
    public record BorrowRequestedEvent(string UserId ,Guid BorrowingId, string BookTitle, string UserName, DateTime RequestDate);
}
