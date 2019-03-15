using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Lux.Helpers;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(Lux.iOS.Custom.CustomProgressBarRenderer))]
namespace Lux.iOS.Custom
{

    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            Control.ProgressTintColor = Color.Green.ToUIColor();
            Control.TrackTintColor = Color.FromRgb(182, 231, 233).ToUIColor();// Color..FromHex("#254f5e").ToUIColor();       
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var X = 1.0f;
            var Y = 1.5f;

            CGAffineTransform transform = CGAffineTransform.MakeScale(X, Y);
            this.Transform = transform;

            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;
            this.Layer.CornerRadius = 1; // This is for rounded corners.
        }
    }
}

