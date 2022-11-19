using Garama.ViewModels.AuthViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Garama.Views.Auth
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = DependencyService.Get<WelcomePageViewModel>();
        }
    }
}
