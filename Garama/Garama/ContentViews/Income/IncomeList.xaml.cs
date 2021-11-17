using Garama.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Garama.ContentViews.Income
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncomeList : ContentView
    {
        public IncomeList()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Current.MainPage.Navigation.PushModalAsync(new AddIncomePage());
            }
            catch (Exception ex)
            {

            }
        }
    }
}