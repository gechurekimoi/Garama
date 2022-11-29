using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.Models.AuthModels
{
    public class GenerateTokenResponse
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }

}
