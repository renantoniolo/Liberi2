using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lux.ViewsModels;
using CarouselView.FormsPlugin.Abstractions;
using Lux.Models;
using System.Diagnostics;
using Lux.Interfaces;
using Lux.Helpers;
using Lux.Services;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Acr.UserDialogs;
using System.Linq;
using Lux.Infra;
using System.Net;

namespace Lux.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EPUBPage : ContentPage
	{

        public EPUBViewModel viewModel { get; set; }
        private int pageSearch = 0;
        private Livro livro;

        public EPUBPage (Livro livro)
		{
			InitializeComponent();
            this.livro = livro;
            App.isLeitor = true;

            Icon = (Device.RuntimePlatform == Device.iOS) ? "menu" : null;

            if (Device.RuntimePlatform == Device.Android)
                UserDialogs.Instance.ShowLoading("Carregando...", MaskType.Gradient);

            BindingContext = viewModel = new EPUBViewModel(livro, this);

            // atribue a classe app.xaml para recuperar o valor da percentagem
            App.ePUBViewModel = viewModel; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((EPUBViewModel)BindingContext).LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing(){

            try{

                // pega o percentual de leitura
                App.ePUBViewModel.PercentualLeitura();

            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "EPUBPage", "OnDisappearing()");
            }

        }

        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
                UserDialogs.Instance.HideLoading();
        }

               public void SearchPageSelect(int page)
        {
            pageSearch = page;
           
        }
    }
}