using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Models.AuthModels
{
    public class RequestUserIdForThirdLogin
    {
        public string immutableId { get; set; }
        public string fullNames { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int authProvider { get; set; }
    }

}
