using System;
using Xamarin.Forms;
using Lux.ViewsModels;
using Syncfusion.SfRating.XForms;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;

namespace Lux.Views
{
    public partial class ComentarioPage : ContentPage
    {
        ComentarioViewModel viewModel;
        public ComentarioPage(int id_livro)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");
            NavigationPage.SetBackButtonTitle(this, "");

			this.BindingContext = viewModel = new ComentarioViewModel(id_livro);

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

		protected override void OnAppearing()
        {
            base.OnAppearing();
			((ComentarioViewModel)BindingContext).ThisOnAppearing();
        }

        public void List_OnSelectedComentario(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
                ListViewComentarios.SelectedItem = null;
        }
    }
}
