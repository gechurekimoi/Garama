using Garama.Enums;
using Garama.Models;
using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Garama.Services
{
    public class BaseService
    {
        DatasyncClient Client;

        IRemoteTable<Category> remoteTable;

        IOfflineTable<ToDoItem> OfflineTable;

        public Func<Task<AuthenticationToken>> TokenRequestor;

        LoginService loginService;

        public BaseService()
        {
            loginService = DependencyService.Get<LoginService>();

            TokenRequestor = GetAuthenticationToken;

            //InitializeDataSync();
        }


        public void InitializeDataSync()
        {
            try
            {

                if (DependencyService.Get<DatasyncClient>() != null)
                    Client = DependencyService.Get<DatasyncClient>();


                var OfflineDb = $"{FileSystem.AppDataDirectory}/todoitems.db3";

                var connectionString = new UriBuilder { Scheme = "file", Path = OfflineDb, Query = "?mode=rwc" }.Uri.ToString();
                
                var store = new OfflineSQLiteStore(connectionString);

                var options = new DatasyncClientOptions
                {
                    OfflineStore = store,
                };


                Client = new DatasyncClient(ApiDetail.PublicEndPoint, new GenericAuthenticationProvider(TokenRequestor), options);

                remoteTable = Client.GetRemoteTable<Category>();

                OfflineTable = Client.GetOfflineTable<ToDoItem>();

                DependencyService.RegisterSingleton<DatasyncClient>(Client);

            }
            catch (Exception ex)
            {

            }
        }


        public Task<AuthenticationToken> GetAuthenticationToken()
        {
            try
            {
                var AuthMethodString = Preferences.Get(nameof(PreferencesConstants.AuthMethod),"");

                if(!string.IsNullOrEmpty(AuthMethodString))
                {
                    //Check if token is from Microsoft
                    var AuthMethod = (AuthMethodEnums)Enum.Parse(typeof(AuthMethodEnums),AuthMethodString);

                    if(AuthMethod == AuthMethodEnums.Microsoft)
                    {
                        return DependencyService.Get<LoginService>().GetAuthenticationTokenForMicrosoft();
                    }
                    else
                    {
                        //here it means auth token is JWT
                       //we first check if Token is Expired

                        string Token = Preferences.Get(nameof(PreferencesConstants.AccessToken), "");

                        bool IsExpired = DependencyService.Get<LoginService>().CheckIfJWTisExpired(Token);

                        if(IsExpired)
                        {
                            // we request another token silently using the refresh token
                            var result  =  DependencyService.Get<LoginService>().GenerateTokenFromRefreshToken().Result;

                            var userDetail = loginService.GetUserDetailsFromTokenClaimsNonMicrosoftLogin(result.accessToken);

                            //we save the token in Preferences
                            Preferences.Set(nameof(PreferencesConstants.AccessToken), result.accessToken);
                            Preferences.Set(nameof(PreferencesConstants.RefreshToken), result.refreshToken);
                            Preferences.Set(nameof(PreferencesConstants.UserId), userDetail.UserId);
                            Preferences.Set(nameof(PreferencesConstants.Name), userDetail.Name);
                            Preferences.Set(nameof(PreferencesConstants.Email), userDetail.Email);
                            Preferences.Set(nameof(PreferencesConstants.PhoneNumber), userDetail.PhoneNumber);
                            Preferences.Set(nameof(PreferencesConstants.AuthMethod), AuthMethodEnums.Jwt.ToString());

                            AuthenticationToken authenticationToken = new AuthenticationToken();

                            authenticationToken.Token = result.accessToken;
                            authenticationToken.UserId = Preferences.Get(nameof(PreferencesConstants.UserId), "");
                            authenticationToken.ExpiresOn = DependencyService.Get<LoginService>().GetExpiryDateJWT(Token);
                            authenticationToken.DisplayName = Preferences.Get(nameof(PreferencesConstants.UserName), "");

                            return Task.FromResult(authenticationToken);
                        }
                        else
                        {
                            AuthenticationToken authenticationToken = new AuthenticationToken();

                            authenticationToken.Token = Token;
                            authenticationToken.UserId = Preferences.Get(nameof(PreferencesConstants.UserId), "");
                            authenticationToken.ExpiresOn = DependencyService.Get<LoginService>().GetExpiryDateJWT(Token);
                            authenticationToken.DisplayName = Preferences.Get(nameof(PreferencesConstants.UserName), "");

                            return Task.FromResult(authenticationToken);
                        }
                    }

                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        public void LogError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
