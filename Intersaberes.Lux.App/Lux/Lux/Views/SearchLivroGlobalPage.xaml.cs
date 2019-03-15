using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class SearchLivroGlobalPage : ContentPage
    {
        public SearchLivroGlobalPage()
        {
            InitializeComponent();

            this.BindingContext = new SearchLivroGlobalViewModel();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((SearchLivroGlobalViewModel)BindingContext).ThisOnAppearing();
        }

        async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ((SearchLivroGlobalViewModel)BindingContext).LoadItemsCommand.Execute(null);
            else
                await ((SearchLivroGlobalViewModel)BindingContext).ItensChanges(e.NewTextValue);
        }
    }
}
