using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public string RefreshToken { get; set; }
      //  public DateTime RefreshTokenExpireOn { get; set; }
        public string Message { get; set; }
    }
}
