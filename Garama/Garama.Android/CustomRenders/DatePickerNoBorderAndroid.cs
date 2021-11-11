using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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

[assembly: ExportRenderer(typeof(DatePickerNoBorder), typeof(DatePickerNoBorderAndroid))]

namespace Garama.Droid.CustomRenders
{
   
    public class DatePickerNoBorderAndroid : DatePickerRenderer
    {
        public DatePickerNoBorderAndroid(Context context) : base(context)
        {

        }

        [Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;

                var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                layoutParams.SetMargins(0, 0, 0, 0);
                LayoutParameters = layoutParams;
                GradientDrawable gd = new GradientDrawable();
                gd.SetStroke(0, Android.Graphics.Color.LightGray);
                Control.SetBackgroundDrawable(gd);
                Control.LayoutParameters = layoutParams;
                Control.SetPadding(0, 0, 0, 0);
                SetPadding(0, 0, 0, 0);
            }

        }


        
    }
}