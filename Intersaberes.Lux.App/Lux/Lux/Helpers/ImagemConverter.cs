using System;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Xamarin.Forms;


namespace Lux.Helpers
{
    public class ImagemConverter : IValueConverter
    {
        ImageSource ConvertImage(string base64) => ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(base64)));

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            return ConvertImage(value.ToString());
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            return ConvertImage(value.ToString());
        }

        public async Task<string> Convertto64(MediaFile file)
        {
            if (file == null) return null;

            var stream = file.GetStream();
            var bytes = new byte[stream.Length];
            await stream.ReadAsync(bytes, 0, (int)stream.Length);
            return System.Convert.ToBase64String(bytes);
        }

        public ImageSource GetImageBase64ToImageSource(string imageBase64)
        {
            ImageSource image = Xamarin.Forms.ImageSource.FromStream(
            () => new MemoryStream(System.Convert.FromBase64String(imageBase64)));

            return image;
        }
    }
}
