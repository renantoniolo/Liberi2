using Lux.Models;
using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class TrocarSenhaPage : ContentPage
    {
        public TrocarSenhaPage(Usuario usuario)
        {
            InitializeComponent();

            this.BindingContext = new TrocarSenhaViewModel(usuario);

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }
    }
}
