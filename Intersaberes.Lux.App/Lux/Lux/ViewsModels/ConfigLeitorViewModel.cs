using System;
using System.Threading.Tasks;

using Lux.Views;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Lux.Helpers;

namespace Lux.ViewsModels
{
    public class ConfigLeitorViewModel : BaseViewModel
    {
        public int FontSize { get; set; }
        public int FontType { get; set; }
        public int ColorType { get; set; }

        public Command ColorCommand { get; set; }
        public Command CloseCommand { get; set; }
        public Command FontTypeCommand { get; set; }
        private EPUBViewModel ePUBViewModel;

        public ConfigLeitorViewModel(int FontSize, int ColorType, int FontType, EPUBViewModel ePUBViewModel)
        {
            this.FontSize = FontSize;
            this.FontType = FontType;
            this.ColorType = ColorType;
            this.ePUBViewModel = ePUBViewModel;

            FontTypeCommand = new Command((object obj) => ChangeFontType(obj));
            ColorCommand = new Command(SetColor);
            CloseCommand = new Command(async () => await Close());
        }

        public void ChangeFont(int value)
        {
            try
            {
                FontSize = value;
                ePUBViewModel.ChangeProperty(FontSize, ColorType, FontType);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "ConfigLeitorViewModel", "ChangeFont()");
            }
        }

        private void ChangeFontType(object obj)
        {
            try
            {
                FontType = Convert.ToInt32(obj);
                ePUBViewModel.ChangeProperty(FontSize, ColorType, FontType);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "ConfigLeitorViewModel", "ConfigLeitorViewModel()");
            }
        }

        void SetColor(object color)
        {
            try
            {
                ColorType = Convert.ToInt32(color);
                ePUBViewModel.ChangeProperty(FontSize, ColorType, FontType);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "ConfigLeitorViewModel", "ConfigLeitorViewModel()");
            }
        }
        async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}
