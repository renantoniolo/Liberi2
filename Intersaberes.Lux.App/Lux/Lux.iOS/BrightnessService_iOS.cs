using System;
using Lux.Interfaces;
using Lux.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BrightnessService_iOS))]
namespace Lux.iOS
{
    public class BrightnessService_iOS : IBrightnessService
    {
        public BrightnessService_iOS()
        {
        }

        public float GetBrightness()
        {
            return (float)UIScreen.MainScreen.Brightness;
        }

        public void SetBrightness(float brightness)
        {
            UIScreen.MainScreen.Brightness = brightness;
        }
    }
}
