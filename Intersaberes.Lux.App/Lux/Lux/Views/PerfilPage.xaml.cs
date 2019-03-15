using Xamarin.Forms;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;

namespace Lux.Views
{
    public partial class PerfilPage : ContentPage
    {
        public PerfilPage(MasterViewModel masterViewModel)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");

            this.BindingContext = new PerfilViewModel(masterViewModel);

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((PerfilViewModel)BindingContext).ThisOnAppearing();
        }
    }
}
