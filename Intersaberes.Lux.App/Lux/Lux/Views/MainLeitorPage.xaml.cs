using System;
using System.Diagnostics;
using AppLeitor;
using Lux.Helpers;
using Lux.Models;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainLeitorPage : MasterDetailPage
    {
        public MainLeitorPage(string caminho, Livro livro)
        {
            InitializeComponent();
            Icon = (Device.RuntimePlatform == Device.iOS) ? "menu" : null;

            // le o livro
            Leitor.LerDiretorio(caminho);

            try
            {
                this.Master = new MasterLeitorPage(livro);
                this.Detail = new NavigationPage(new EPUBPage(livro))
                {
                    BarBackgroundColor = Color.FromHex("#C9C9CE"),
                    BarTextColor = Color.FromHex("#737373"),
                };
            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "MainLeitorPage", "Contrutor()");
            }

            NavigationPage.SetHasNavigationBar(this, false);
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Undefined);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // bloqueia o gesture do menu slider
            if((Device.RuntimePlatform == Device.iOS)) IsGestureEnabled = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // desbloqueia o gesture do menu slider
            if ((Device.RuntimePlatform == Device.iOS)) IsGestureEnabled = true;
        }

    }
}