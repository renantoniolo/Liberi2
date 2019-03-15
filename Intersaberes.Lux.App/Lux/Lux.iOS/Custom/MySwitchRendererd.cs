using System;
using Lux.Helpers;
using Lux.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(MySwitchRendererd))]
namespace Lux.iOS.Custom
{
    public class MySwitchRendererd : SwitchRenderer
    {
        CustomSwitch s;
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            this.s = Element as CustomSwitch;

            if (s == null) return;

            if(s.IsToggled){
                Control.ThumbTintColor = s.SwitchThumbColor.ToUIColor();
                Control.OnTintColor = s.SwitchOnColor.ToUIColor();
            }
            else{
                Control.ThumbTintColor = s.SwitchThumbColor.ToUIColor();

            }

            Control.ValueChanged += Control_ValueChanged;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            Control.ThumbTintColor = s.SwitchThumbColor.ToUIColor();
            Control.OnTintColor = s.SwitchOnColor.ToUIColor();
        }

        protected override void Dispose(bool disposing)
        {
            Control.ValueChanged -= Control_ValueChanged;
            base.Dispose(disposing);
        }

    }

}
