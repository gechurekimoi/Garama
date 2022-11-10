using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Garama.Infrastructure.Services.Token
{
    public class TokenService : ITokenService
    {
        ILogger logger;
        public TokenService(ILogger _logger)
        {
            logger = _logger;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, string Issuer, string Audience, string SigningKey)
        {
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));

                var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer:Issuer,
                    audience: Audience,
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signInCredentials);

                var stringToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return stringToken;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating token");
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];

                using(var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);

                    return Convert.ToBase64String(randomNumber);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating refresh token");
                return null;
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string SigningKey)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error generating refresh token");
                return null;
            }
        }
    }
}
