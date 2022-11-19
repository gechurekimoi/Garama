using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garama.Domain.Common.Auth
{
    public class ThirdPartyAuthUserDetails
    {
        public string ImmutableId { get; set; }
        public string FullNames { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int AuthProvider { get; set; }

    }
}
