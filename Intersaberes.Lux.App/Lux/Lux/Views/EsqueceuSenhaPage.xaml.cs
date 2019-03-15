using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class EsqueceuSenhaPage : ContentPage
    {
        public EsqueceuSenhaPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            this.BindingContext = new EsqueceuSenhaViewModel();

            // bloquear para Portrait sempre para login
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }
    }
}
