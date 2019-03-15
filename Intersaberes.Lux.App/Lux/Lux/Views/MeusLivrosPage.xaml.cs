using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class MeusLivrosPage : ContentPage
    {
        public MeusLivrosPage()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");
            NavigationPage.SetBackButtonTitle(this, "");

            this.BindingContext = new MeusLivrosViewModel();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MeusLivrosViewModel)BindingContext).ThisOnAppearing();
        }

        async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ((MeusLivrosViewModel)BindingContext).LoadItemsCommand.Execute(null);
            else
                await ((MeusLivrosViewModel)BindingContext).ItensChanges(e.NewTextValue);
        }
    }
}
