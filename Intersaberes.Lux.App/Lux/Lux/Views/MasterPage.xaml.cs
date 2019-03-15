using System;
using Lux.ViewsModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();

            Icon = (Device.RuntimePlatform == Device.iOS) ? "menu" : null;

            BindingContext = new MasterViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.isLeitor = false;
            ((MasterViewModel)BindingContext).ThisOnAppearing();
        }

        void OnSairClicked(Object sender, EventArgs e) => App.MasterDetail.SendBackButtonPressed();
    }
}