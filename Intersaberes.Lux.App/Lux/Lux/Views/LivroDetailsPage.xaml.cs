using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Lux.Models;
using Lux.ViewsModels;
using Lux.Interfaces;
using Plugin.DeviceOrientation;
using Plugin.DeviceOrientation.Abstractions;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Lux.Services;
using Lux.Infra;
using System.Collections.ObjectModel;
using Lux.Helpers;

namespace Lux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LivroDetailsPage : ContentPage
    {
        private int idLivro;

        public LivroDetailsPage(Livro item)
        {
            InitializeComponent();

            idLivro = (int)item.Id;

            if (Device.RuntimePlatform == Device.iOS) NavigationPage.SetTitleIcon(this, "liberi_logo_topo");
            NavigationPage.SetBackButtonTitle(this, "");

            BindingContext = new LivroDetailsViewModel(item);

            CrossDeviceOrientation.Current.LockOrientation(DeviceOrientations.Portrait);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            DependencyService.Get<IConfig>().DirectoryDeleteAll();
            ((LivroDetailsViewModel)BindingContext).ThisOnAppearing();
        }



    }
}