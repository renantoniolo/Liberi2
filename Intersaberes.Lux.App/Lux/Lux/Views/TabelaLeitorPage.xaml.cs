using System.Collections.Generic;
using Lux.ViewsModels;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class TabelaLeitorPage : ContentPage
    {
        public TabelaLeitorPage(List<string> listTabelas)
        {
            InitializeComponent();

            BindingContext = new TabelaViewModel(listTabelas);
        }

        protected override void OnAppearing()
        {
            /*
            if (Device.RuntimePlatform == Device.iOS)
                CarouselAndroid.IsVisible = false;
            else
                CarouseliOS.IsVisible = false;
                */
                
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}
