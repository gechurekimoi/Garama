using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Garama.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddIncomePage : ContentPage
    {
        public AddIncomePage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}