using System;
using System.Collections.Generic;
using CarouselView.FormsPlugin.Abstractions;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lux.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudioPop : PopupPage
    {
        public AudioPop(List<string> listAudios)
        {
            InitializeComponent();
            BindingContext = new AudioViewModel(listAudios);

            // bloquear para Portrait sempre para login
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
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

        void OnValueChanged(object sender, ValueChangedEventArgs args)
        {
            //int id = Convert.ToInt32((sender as Slider).ClassId);

            //if (Math.Abs(args.NewValue - args.OldValue) > 2)
            //    ((AudioViewModel)BindingContext).SetPositionPlay(id, args.NewValue);
        }

        public void OnCarroselPositionSelected(object sender, PositionSelectedEventArgs e)
        {
            ((AudioViewModel)BindingContext).SetArrow(e.NewValue);
        }
    }
}