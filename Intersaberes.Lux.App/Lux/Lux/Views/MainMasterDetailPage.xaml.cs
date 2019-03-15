using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Lux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMasterDetailPage : MasterDetailPage
    {
        public MainMasterDetailPage()
        {
            InitializeComponent();

            Icon = (Device.RuntimePlatform == Device.iOS) ? "ic_menu" : null;

            // essa é minha masterDetail principal
            App.MasterDetail = this;

            this.Master = new MasterPage();
            this.Detail = new NavigationPage(new HomePage());

            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}