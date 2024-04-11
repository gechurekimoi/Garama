using Garama.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Garama.ContentViews.Expenses
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseListView : ContentView
    {
        public ExpenseListView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new AddExpensePage());
        }
    }
}