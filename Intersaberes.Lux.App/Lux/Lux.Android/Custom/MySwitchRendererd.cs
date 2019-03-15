using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Lux.Helpers;
using Android.Graphics;
using Android.Widget;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(Lux.Droid.Custom.MySwitchRendererd))]
namespace Lux.Droid.Custom
{
    public class MySwitchRendererd : SwitchRenderer
    {
        public MySwitchRendererd(Context context) : base(context) { }
        CustomSwitch s;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);

            s = Element as CustomSwitch;

            if (s == null) return;

            s.Toggled += S_Toggled;

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {

                if (s.IsToggled)
                {
                    this.Control.TrackDrawable.SetColorFilter(s.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
                else
                {
                    this.Control.TrackDrawable.SetColorFilter(s.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                }
 
            }

        }

        void S_Toggled(object sender, ToggledEventArgs e)
        {
            if (s.IsToggled)
            {
                this.Control.TrackDrawable.SetColorFilter(s.SwitchOnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }
            else
            {
                this.Control.TrackDrawable.SetColorFilter(s.SwitchThumbColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
            }

        }
   
        protected override void Dispose(bool disposing)  
        {  
            s.Toggled -= S_Toggled;
            base.Dispose(disposing);  
        }  

    }

}