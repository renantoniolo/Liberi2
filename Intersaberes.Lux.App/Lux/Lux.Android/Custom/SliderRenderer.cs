using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Lux.Helpers.CustomSlider), typeof(Lux.Droid.Custom.SlideCustom))]
namespace Lux.Droid.Custom
{
    public class SlideCustom : SliderRenderer
    {
        public SlideCustom() : base(null) => throw new Exception("This constructor should not actually ever be used");

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Slider> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;

            var view = (Helpers.CustomSlider)Element;
            if (!string.IsNullOrEmpty(view.ThumbImage) ||
            view.ThumbColor != Xamarin.Forms.Color.Default ||
                view.MaxColor != Xamarin.Forms.Color.Default ||
                view.MinColor != Xamarin.Forms.Color.Default)
            {
                // Set Thumb Icon
                Control.SetThumb(Resources.GetDrawable(view.ThumbImage));
            }

            Control.Thumb.SetColorFilter(view.ThumbColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            Control.ProgressTintList = Android.Content.Res.ColorStateList.ValueOf(view.MinColor.ToAndroid());
            Control.ProgressTintMode = PorterDuff.Mode.SrcIn;
            //this is for Maximum Slider line Color
            Control.ProgressBackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(view.MaxColor.ToAndroid());
            Control.ProgressBackgroundTintMode = PorterDuff.Mode.SrcIn;

            Control.ProgressChanged += delegate (object sender, SeekBar.ProgressChangedEventArgs args)
            {
                if (args.FromUser)
                    System.Diagnostics.Debug.WriteLine("Usuario ativou seek bar");
            };
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                if (Control == null) return;

                SeekBar ctrl = Control;
                Drawable thumb = ctrl.Thumb;
                int thumbTop = ctrl.Height / 2 - thumb.IntrinsicHeight / 2;

                thumb.SetBounds(thumb.Bounds.Left, thumbTop,
                                thumb.Bounds.Left + thumb.IntrinsicWidth, thumbTop + thumb.IntrinsicHeight);
            }
        }
    }
}
