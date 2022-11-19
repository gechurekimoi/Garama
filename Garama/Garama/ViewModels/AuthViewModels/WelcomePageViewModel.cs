using Garama.Views.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Garama.ViewModels.AuthViewModels
{
    public class WelcomePageViewModel : BaseViewModel
    {
        public ICommand ICreateAccountCommand { get; set; }
        public ICommand ILoginCommand { get; set; }


        public WelcomePageViewModel()
        {
            ICreateAccountCommand = new Command(async()=> await CreateAccountCommand());

            ILoginCommand = new Command(async()=> await LoginCommand());

        }


        public async Task CreateAccountCommand()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new SignUpPage());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("",ex);
            }
        }

        public async Task LoginCommand()
        {
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            catch (Exception ex)
            {
                ShowErrorMessage("", ex);
            }
        }
    }
}
