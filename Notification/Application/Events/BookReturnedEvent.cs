using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Events
{
    public record BookReturnedEvent(string BookTitle, string UserId ,string UserName, DateTime ReturnDate, decimal LateFees, string Message);
}
