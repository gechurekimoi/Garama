using System;
using System.Collections.Generic;
using System.Text;

namespace Garama.ViewModels.AuthViewModels
{
    public class LoginPageViewModel  : BaseViewModel
    {

		private string phoneNumber;
		public string PhoneNumber
		{
			get { return phoneNumber; }
			set { phoneNumber = value; OnPropertyChanged(); }
		}


		private string pin;
		public string Pin
		{
			get { return pin; }
			set { pin = value; OnPropertyChanged(); }
		}





	}
}
