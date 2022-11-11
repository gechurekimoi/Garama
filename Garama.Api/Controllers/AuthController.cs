using Garama.Domain.Common.Auth;
using Garama.Infrastructure.Services;
using Garama.Infrastructure.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Garama.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration Configuration;
        private ITokenService tokenService;
        private IUserService userService;

        public AuthController(IConfiguration configuration,
            ITokenService tokenService,
            IUserService userService)
        {
            this.Configuration= configuration;
            this.tokenService = tokenService;
            this.userService = userService;
        }

        [HttpPost]
        [Route("GenerateToken")]
        public ActionResult GenerateAuthToken(LoginModel loginModel)
        {

            if (loginModel is null)
                return BadRequest();

            var user = userService.GetUserDetails(loginModel);

            if (user is null)
                return Unauthorized();

            string IssuerSigningKey = Configuration["PhoneAuthenticationToken:IssuerSigningKey"].ToString();
           
            string Issuer = Configuration["PhoneAuthenticationToken:ValidIssuer"].ToString();
            
            string Audience = Configuration["PhoneAuthenticationToken:ValidAudience"].ToString();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(type:"UserId", user.Id),
                new Claim(type:"Name", user.FullName),
                new Claim(type:"Email", user.Email),
                new Claim(type:"PhoneNumber", user.PhoneNumber)
            };

            string AccessToken = tokenService.GenerateAccessToken(claims, Issuer, Audience, IssuerSigningKey);
            
            string RefreshToken = tokenService.GenerateRefreshToken();

            userService.UpdateRefreshToken(RefreshToken, user.Id);

            TokenResponse tokenReponse = new TokenResponse()
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
            
            if (AccessToken is null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Logged in successfully but failed to generate token");

            return Ok(tokenReponse);

        }


        [HttpPost]
        [Route("RefreshToken")]
        public ActionResult RefreshToken(TokenResponse tokenResponse)
        {
            if (tokenResponse is null)
                return BadRequest();

            string IssuerSigningKey = Configuration["PhoneAuthenticationToken:IssuerSigningKey"].ToString();

            string Issuer = Configuration["PhoneAuthenticationToken:ValidIssuer"].ToString();

            string Audience = Configuration["PhoneAuthenticationToken:ValidAudience"].ToString();

            var principal = tokenService.GetPrincipalFromExpiredToken(tokenResponse.AccessToken, IssuerSigningKey);

            var claims = principal.Claims.ToList();

            string UserId = claims[0].Value;//Where(p=>p.Subject.ToString()=="UserId").ToString();

            var user = userService.GetUserById(UserId);

            if (user.RefreshToken != tokenResponse.RefreshToken)
                return Unauthorized();

            //we generate a new token and refresh token
          

            

            List<Claim> NewClaims = new List<Claim>()
            {
                new Claim(type:"UserId", user.Id),
                new Claim(type:"Name", user.FullName),
                new Claim(type:"Email", user.Email),
                new Claim(type:"PhoneNumber", user.PhoneNumber)
            };

            string AccessToken = tokenService.GenerateAccessToken(NewClaims, Issuer, Audience, IssuerSigningKey);

            string RefreshToken = tokenService.GenerateRefreshToken();

            userService.UpdateRefreshToken(RefreshToken, user.Id);

            TokenResponse tokenReponse = new TokenResponse()
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };

            if (AccessToken is null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Logged in successfully but failed to generate token");

            return Ok(tokenReponse);

        }


        [Authorize]
        [HttpGet("TestAuthToken")]
        public ActionResult TestAuthToken()
        {
            return Ok("works");
        }


    }
}
