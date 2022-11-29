using Garama.Models;
using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.Offline.Queue;
using Microsoft.Datasync.Client.Offline;
using Microsoft.Datasync.Client.SQLiteStore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.AppleSignInAuthenticator;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Garama.Views
{
    public partial class AboutPage : ContentPage
    {
        DatasyncClient Client;
        IRemoteTable<ToDoItem> remoteTable;
        IOfflineTable<ToDoItem> OfflineTable;
        public Func<Task<AuthenticationToken>> TokenRequestor;
        public IPublicClientApplication IdentityClient { get; set; }
        public IPlatform PlatformService { get; set; }

        private ObservableCollection<ToDoItem> toDoItems;

        public ObservableCollection<ToDoItem> ToDoItems
        {
            get { return toDoItems; }
            set { toDoItems = value; OnPropertyChanged(); }
        }


        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this;
            PlatformService = Constants.PlatformService;
            TokenRequestor = GetAuthenticationToken;
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                //await InitializeDataSync();
                //await GetDataFromDatabase();
            });

        }

        public async Task SyncAsync()
        {
            ReadOnlyCollection<TableOperationError> syncErrors = null;

            try
            {

                await OfflineTable.PushItemsAsync();
                await OfflineTable.PullItemsAsync("");

            }
            catch (PushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    //syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == TableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
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


            var deserilizedToken = new JwtSecurityToken(result.AccessToken);

            var claims = deserilizedToken.Claims.ToList();


            return new AuthenticationToken
            {
                DisplayName = result?.Account?.Username ?? "",
                ExpiresOn = result?.ExpiresOn ?? DateTimeOffset.MinValue,
                Token = result?.AccessToken ?? "",
                UserId = result?.Account?.Username ?? ""
            };

        }

        public async Task InitializeDataSync()
        {
            try
            {
                var OfflineDb = $"{FileSystem.AppDataDirectory}/todoitems.db3";
                var connectionString = new UriBuilder { Scheme = "file", Path = OfflineDb, Query = "?mode=rwc" }.Uri.ToString();
                var store = new OfflineSQLiteStore(connectionString);

                var options = new DatasyncClientOptions
                {
                    OfflineStore = store,
                };


                Client = new DatasyncClient("https://garama.azurewebsites.net", new GenericAuthenticationProvider(TokenRequestor), options);

                remoteTable = Client.GetRemoteTable<ToDoItem>();

                OfflineTable = Client.GetOfflineTable<ToDoItem>();

            }
            catch (Exception ex)
            {

            }
        }

        public async Task GetDataFromDatabase()
        {
            try
            {
                var data = new ObservableCollection<ToDoItem>();

                var items = await remoteTable.ToAsyncEnumerable().ToListAsync();

                foreach (var item in items)
                {
                    data.Add(item);
                }


                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems = data;
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await InitializeDataSync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await TokenRequestor();
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            await GetDataFromDatabase();
        }
    }
}