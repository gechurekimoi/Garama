using Garama.Services;
using Garama.ViewModels.AuthViewModels;
using Garama.Views;
using Garama.Views.Auth;
using Microsoft.Datasync.Client;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Garama
{
    public partial class App : Application
    {
        public IPublicClientApplication IdentityClient { get; set; }
        public IPlatform PlatformService { get; }

        public App(IPlatform platformService)
        {
            InitializeComponent();

            PlatformService = platformService;

            Constants.PlatformService = platformService;

            DependencyService.Register<MockDataStore>();

            AddViewModelsToDepedencyService();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDg3MjI3QDMxMzkyZTMyMmUzMEdKcTBINUEwaTlyT1pQT1ZEOGdhc1NCbTAyN0NsbjBhRk1zWkRYaE1LUzA9");

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new WelcomePage());
            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public async Task<AuthenticationToken> GetAuthenticationToken()
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

            return new AuthenticationToken
            {
                DisplayName = result?.Account?.Username ?? "",
                ExpiresOn = result?.ExpiresOn ?? DateTimeOffset.MinValue,
                Token = result?.AccessToken ?? "",
                UserId = result?.Account?.Username ?? ""
            };
        }

        public void AddViewModelsToDepedencyService()
        {
            try
            {
                DependencyService.Register<WelcomePageViewModel>();
            }
            catch (Exception ex)
            {

            }
        } 
    }
}
