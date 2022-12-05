using Garama.Models.AuthModels;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Garama.Services
{
    public class LoginService : BaseService
    {

        public IPublicClientApplication IdentityClient { get; set; }
        public IPlatform PlatformService { get; set; }

        public RequestUserIdForThirdLogin RequestUserIdForThirdLogin = new RequestUserIdForThirdLogin();

        public LoginService()
        {
            PlatformService = Constants.PlatformService;
        }


        public async Task<GenerateTokenResponse> GenerateTokenForPinOrPassword(GenerateToken userDetails)
        {
			try
			{

                RestClient restClient= new RestClient(ApiDetail.EndPoint);

                RestRequest restRequest = new RestRequest()
                {
                    Resource = "/api/Auth/GenerateToken"
                };

                restRequest.AddJsonBody(userDetails);

                var response = await restClient.PostAsync(restRequest);

                if (!response.IsSuccessful)
                    return null;

                var deserializeResponse = JsonConvert.DeserializeObject<GenerateTokenResponse>(response.Content);

                return deserializeResponse;
               
			}
			catch (Exception ex)
			{
                LogError(ex);
                return null;
			}
        }

        public async Task<AuthenticationToken> GetAuthenticationTokenForMicrosoft()
        {
            try
            {
                if (IdentityClient == null)
                {
                    IdentityClient = PlatformService.GetIdentityClient(Constants.ApplicationId);
                }

                var accounts = await IdentityClient.GetAccountsAsync();
                AuthenticationResult result = null;
                bool tryInteractiveLogin = false;

                try
                {
                    result = await IdentityClient
                        .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                        .ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    tryInteractiveLogin = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"MSAL Silent Error: {ex.Message}");
                }

                if (tryInteractiveLogin)
                {
                    try
                    {
                        result = await IdentityClient
                            .AcquireTokenInteractive(Constants.Scopes)
                            .ExecuteAsync()
                            .ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"MSAL Interactive Error: {ex.Message}");
                    }
                }

                SetThirdPartyValuesForMicrosoftLogin(result.AccessToken);

                return new AuthenticationToken
                {
                    DisplayName = result?.Account?.Username ?? "",
                    ExpiresOn = result?.ExpiresOn ?? DateTimeOffset.MinValue,
                    Token = result?.AccessToken ?? "",
                    UserId = result?.Account?.Username ?? ""
                };
            }
            catch (Exception mainEx)
            {

                LogError(mainEx);
                return new AuthenticationToken
                {
                    DisplayName = null,
                    ExpiresOn = DateTime.Now,
                    Token = null,
                    UserId = null
                };
            }

        }

        public JwtTokenUserDetail GetUserDetailsFromTokenClaimsNonMicrosoftLogin(string Token)
        {
            try
            {
                JwtTokenUserDetail userDetail = new JwtTokenUserDetail();

                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);

                var claims = jwtSecurityToken.Claims.ToList();

                userDetail.UserId = claims.Where(p=>p.Type == "UserId").FirstOrDefault().Value;
                userDetail.Name = claims.Where(p=>p.Type == "Name").FirstOrDefault().Value;
                userDetail.Email = claims.Where(p=>p.Type == "Email").FirstOrDefault().Value;
                userDetail.PhoneNumber = claims.Where(p=>p.Type == "PhoneNumber").FirstOrDefault().Value;


                return userDetail;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public async Task<JwtTokenUserDetail> GetUserIdForMicrosoftAuthUser(RequestUserIdForThirdLogin requestUser,string Token)
        {
            try
            {
                RestClient restClient = new RestClient(ApiDetail.EndPoint);

                restClient.Authenticator = new JwtAuthenticator(Token);

                RestRequest restRequest = new RestRequest()
                {
                    Resource = "/api/Auth/AddThirdPartyUserToDbAndGetUserId"
                };

                restRequest.AddJsonBody(requestUser);

                var response = await restClient.PostAsync(restRequest);

                if (!response.IsSuccessful)
                    return null;

                var deserializeResponse = JsonConvert.DeserializeObject<string>(response.Content);

                JwtTokenUserDetail userDetail = new JwtTokenUserDetail();

                userDetail.UserId = deserializeResponse;
                userDetail.Name = requestUser.fullNames;
                userDetail.Email = requestUser.email;
                userDetail.PhoneNumber= requestUser.phoneNumber;
                

                return userDetail;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }


        private void SetThirdPartyValuesForMicrosoftLogin(string Token)
        {
            try
            {
                var deserilizedToken = new JwtSecurityToken(Token);

                var claims = deserilizedToken.Claims;

                RequestUserIdForThirdLogin.immutableId = claims.Where(p => p.Type == "oid").FirstOrDefault().Value;
                RequestUserIdForThirdLogin.fullNames = claims.Where(p => p.Type == "name").FirstOrDefault().Value;
                RequestUserIdForThirdLogin.email = claims.Where(p => p.Type == "preferred_username").FirstOrDefault().Value;

            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public bool CheckIfJWTisExpired(string Token)
        {
            try
            {

                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);

                var claims = jwtSecurityToken.Claims.ToList();

                var expiryDateString = claims.Where(p => p.Type == "exp").FirstOrDefault().Value;

                var ExpiryDate = Convert.ToDateTime(expiryDateString);

                if (DateTime.Now > ExpiryDate)
                    return true;


                return false;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return true;
            }
        }

        public DateTime GetExpiryDateJWT(string Token)
        {
            try
            {

                var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);

                var claims = jwtSecurityToken.Claims.ToList();

                var expiryDateString = claims.Where(p => p.Type == "exp").FirstOrDefault().Value;

                var ExpiryDate = Convert.ToDateTime(expiryDateString);

                return ExpiryDate;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return DateTime.Now;
            }
        }

        public async Task<GenerateTokenResponse> GenerateTokenFromRefreshToken()
        {
            try
            {

                RestClient restClient = new RestClient(ApiDetail.EndPoint);

                RestRequest restRequest = new RestRequest()
                {
                    Resource = "/api/Auth/RefreshToken"
                };

                RefreshTokenBody requestBody = new RefreshTokenBody();
                requestBody.accessToken = Preferences.Get(nameof(PreferencesConstants.AccessToken), "");
                requestBody.refreshToken = Preferences.Get(nameof(PreferencesConstants.RefreshToken), "");

                restRequest.AddJsonBody(requestBody);

                var response = await restClient.PostAsync(restRequest);

                if (!response.IsSuccessful)
                    return null;

                var deserializeResponse = JsonConvert.DeserializeObject<GenerateTokenResponse>(response.Content);

                return deserializeResponse;

            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }
    }
}
