using System;
using Xamarin.Forms;

namespace Lux.Helpers
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "0";
            return (System.Convert.ToDouble(value) / 100);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "0";
            return (System.Convert.ToDouble(value) / 100);
        }
    }
}
