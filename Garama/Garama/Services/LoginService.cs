using Garama.Models.AuthModels;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Garama.Services
{
    public class LoginService : BaseService
    {

        public IPublicClientApplication IdentityClient { get; set; }
        public IPlatform PlatformService { get; set; }

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


                var deserilizedToken = new JwtSecurityToken(result.AccessToken);


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
    }
}
