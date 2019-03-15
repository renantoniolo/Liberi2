using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Lux.Helpers;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(Lux.Droid.Custom.MyProgressBarRenderer))]
namespace Lux.Droid.Custom
{
    public class MyProgressBarRenderer : ProgressBarRenderer
    {
        public MyProgressBarRenderer(Context context) : base(context) { }

        public MyProgressBarRenderer() : base(null) => throw new Exception("This constructor should not actually ever be used");

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Lollipop)//API 21(5.0)
            {
                Control.ProgressDrawable.SetColorFilter(Color.FromRgb(182, 231, 233).ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcIn);
                Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(Color.Green.ToAndroid());//ColorStateList Android >= 5.0
            }
        }
    }
}