using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            this.BindingContext = new CadastroViewModel();
        }
    }
}
