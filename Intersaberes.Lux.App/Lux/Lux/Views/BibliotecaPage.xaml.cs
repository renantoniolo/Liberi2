using Lux.ViewsModels;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lux.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BibliotecaPage : ContentPage
	{

		public BibliotecaPage()
		{
			InitializeComponent();

            if(Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");
            NavigationPage.SetBackButtonTitle(this, "");

            BindingContext = new BibliotecaViewModel();

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((BibliotecaViewModel)BindingContext).ThisOnAppearing();
        }

        async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ((BibliotecaViewModel)BindingContext).LoadItemsCommand.Execute(null);
            else
                await ((BibliotecaViewModel)BindingContext).ItensChanges(e.NewTextValue);
        }
    }
}
