using System;

using Lux.Interfaces;
using Lux.ViewsModels;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;

namespace Lux.Views.Popup
{
    public partial class ConfigLeitorPop : PopupPage
    {
        ConfigLeitorViewModel viewModel;
        public ConfigLeitorPop(int FontSize, int ColorType, int FontType, EPUBViewModel ePUBViewModel)
        {
            InitializeComponent();
            BindingContext = viewModel = new ConfigLeitorViewModel(FontSize, ColorType, FontType, ePUBViewModel);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }


        //Eventos Sliders
        void OnChangedFont(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            int value = Convert.ToInt32(slider.Value);
            viewModel.ChangeFont(value);
        }
       
        void OnChangedBrilho(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            float value = (float)slider.Value;

            DependencyService.Get<IBrightnessService>().SetBrightness(value);
        }

    }
}