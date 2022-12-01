using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Models.AuthModels
{
    public class JwtTokenUserDetail
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
