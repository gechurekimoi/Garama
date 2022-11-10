using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Garama.Infrastructure.Services.Token
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims, string Issuer, string Audience, string SigningKey);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string SigningKey);
    }
}
