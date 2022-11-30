using Garama.Models.AuthModels;
using Garama.Services;
using Microsoft.Identity.Client;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Garama.ViewModels.AuthViewModels
{
    public class LoginPageViewModel  : BaseViewModel
    {

		private string userName;
		public string UserName
		{
			get { return userName; }
			set { userName = value; OnPropertyChanged(); }
		}


		private string pin;
		public string Pin
		{
			get { return pin; }
			set { pin = value; OnPropertyChanged(); }
		}

		public ICommand LoginWithUserPinOrPasswordCommand { get; set; }
		public ICommand LoginWithMicrosoftCommand { get; set; }

		public LoginService loginService { get; set; }

		public LoginPageViewModel()
		{
			loginService = DependencyService.Get<LoginService>();

			LoginWithUserPinOrPasswordCommand = new Command(async () => await LoginWithUserPinOrPassword());
			
			LoginWithMicrosoftCommand = new Command(async () => await LoginWithMicrosoft());

        }


		public async Task LoginWithUserPinOrPassword()
		{
			try
			{

				GenerateToken generateToken = new GenerateToken()
				{
					userName = userName,
					password = pin
				};

				var generateTokenResult = await loginService.GenerateTokenForPinOrPassword(generateToken);

				if(generateTokenResult != null)
				{
					//we save the token in Preferences
					Preferences.Set(nameof(PreferencesConstants.AccessToken), generateTokenResult.accessToken);
					Preferences.Set(nameof(PreferencesConstants.RefreshToken), generateTokenResult.refreshToken);
					Preferences.Set(nameof(PreferencesConstants.UserId), "");


					App.Current.MainPage = new AppShell();
				}
				else
				{
                    //we display error message
                    ShowErrorMessage("Error when trying to Login, please try again", null);
                }

			}
			catch (Exception ex)
			{
				ShowErrorMessage("Error when trying to Login, please try again", ex);
			}
		}



		public async Task LoginWithMicrosoft()
		{
			try
			{

				var result = await loginService.GetAuthenticationTokenForMicrosoft();

				if(!string.IsNullOrEmpty(result.Token))
				{
                    App.Current.MainPage = new AppShell();
				}
				else
				{
                    //we display error message
                    ShowErrorMessage("Error when trying to Login, please try again", null);
                }


			}
			catch (Exception ex)
			{

				ShowErrorMessage("Something went wrong when trying to sign in", ex);
			}
		}

    }
}
