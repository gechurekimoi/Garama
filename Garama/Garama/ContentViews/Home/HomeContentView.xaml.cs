using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Garama.ContentViews.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeContentView : ContentView
    {
        public HomeContentView()
        {
            InitializeComponent();
        }
    }
}