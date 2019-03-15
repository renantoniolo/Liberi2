using System;
using Xamarin.Forms;
using Syncfusion.SfRating.XForms;

namespace Lux.Helpers
{
    public class DoubleToRattings : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SfRatingSettings { RatedFill = Color.Red, RatedStroke = Color.Red };

            if ((double)value < 2)
                return new SfRatingSettings() { RatedFill = Color.Red, RatedStroke = Color.Red };
            else if ((double)value <= 3)
                return new SfRatingSettings() { RatedFill = Color.Yellow, RatedStroke = Color.Yellow };
            else
                return new SfRatingSettings() { RatedFill = Color.Green, RatedStroke = Color.Green };
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return new SfRatingSettings { RatedFill = Color.Red, RatedStroke = Color.Red };

            if ((double)value < 2)
                return new SfRatingSettings() { RatedFill = Color.Red, RatedStroke = Color.Red };
            else if ((double)value <= 3)
                return new SfRatingSettings() { RatedFill = Color.Yellow, RatedStroke = Color.Yellow };
            else
                return new SfRatingSettings() { RatedFill = Color.Green, RatedStroke = Color.Green };
        }
    }
}
