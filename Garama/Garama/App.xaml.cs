using Garama.Services;
using Garama.Views;
using Garama.Views.Auth;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Garama
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDg3MjI3QDMxMzkyZTMyMmUzMEdKcTBINUEwaTlyT1pQT1ZEOGdhc1NCbTAyN0NsbjBhRk1zWkRYaE1LUzA9");

            //MainPage = new AppShell();
            MainPage = new Views.Auth.SignUpPage();
            
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
    }
}
