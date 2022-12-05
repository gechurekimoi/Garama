using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Models.AuthModels
{
    public class RefreshTokenBody
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}
