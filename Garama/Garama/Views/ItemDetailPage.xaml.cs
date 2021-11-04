using Garama.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Garama.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}