using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Garama.CustomRenders;
using Garama.Droid.CustomRenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(EntryNoBorder), typeof(EntryNoBordersAndroid))]

namespace Garama.Droid.CustomRenders
{
    public class EntryNoBordersAndroid : EntryRenderer
    {
        public EntryNoBordersAndroid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }
    }
}