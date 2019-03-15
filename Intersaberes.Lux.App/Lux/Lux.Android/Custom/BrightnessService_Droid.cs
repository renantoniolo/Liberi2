using System;
using Android.App;
using Lux.Droid.Custom;
using Lux.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(BrightnessService_Droid))]
namespace Lux.Droid.Custom
{
    public class BrightnessService_Droid : IBrightnessService
    {
        public BrightnessService_Droid()
        {
        }

        public float GetBrightness()
        {
            var windows = ((Activity)Forms.Context).Window;
            var window = CrossCurrentActivity.Current.Activity.Window;

            var layout = window.Attributes;
            var layouts = windows.Attributes;

            return layout.ScreenBrightness;
        }

        public void SetBrightness(float brightness)
        {
            var windows = ((Activity)Forms.Context).Window;
            var window = CrossCurrentActivity.Current.Activity.Window;

            var layout = window.Attributes;
            var layouts = windows.Attributes;

            layout.ScreenBrightness = brightness;

            windows.Attributes = layout;
            window.Attributes = layout;
        }
    }
}
