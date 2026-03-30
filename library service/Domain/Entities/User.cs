using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }

        #region Relationship
        public List<Borrowing> Borrowings { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Fine> Fines { get; set; }
        #endregion
    }
}
