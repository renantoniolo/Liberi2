using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lux.ViewsModels;
using Plugin.DeviceOrientation.Abstractions;
using Plugin.DeviceOrientation;

namespace Lux.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        
		public LoginPage ()
		{
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);


            BindingContext = new LoginViewModel();

            // bloquear para Portrait sempre para login
            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((LoginViewModel)BindingContext).ThisOnAppearing();
        }


    }
}