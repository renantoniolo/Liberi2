using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;

namespace Lux.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
        HomeViewModel viewModel;
        public HomePage ()
		{
			InitializeComponent ();

            Icon = (Device.RuntimePlatform == Device.iOS) ? "ic_menu" : null;
            if (Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");
            NavigationPage.SetBackButtonTitle(this, "");

            this.BindingContext = viewModel = new HomeViewModel();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadCommand.Execute(null);
        }
    }
}