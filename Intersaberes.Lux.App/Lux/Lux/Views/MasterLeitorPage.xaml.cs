using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Lux.Models;
using Lux.ViewsModels;
using Lux.Views.Popup;
using Rg.Plugins.Popup.Extensions;
using Lux.Interfaces;
using System;
using Lux.Helpers;

namespace Lux.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterLeitorPage : TabbedPage
    {

        private bool menuSelecionado;

        public MasterLeitorPage(Livro item)
        {
            InitializeComponent();

            Icon = (Device.RuntimePlatform == Device.iOS) ? "menu" : null;
            BindingContext = new MasterLeitorViewModel(item);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            menuSelecionado = false;

            ((MasterLeitorViewModel)BindingContext).ThisOnAppearingOpcoes();
        }


        protected override void OnDisappearing()
        {

            try
            {
                if (!menuSelecionado)
                {
                    App.ePUBViewModel.PercentualLeitura();
                    // deleta o livro
                    DependencyService.Get<IConfig>().DirectoryDeleteAll();
                }

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MasterLeitorPage", "OnDisappearing()");
            }
            finally
            {
                // clear memory
                System.GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }

        void Search_OnTextOpcoes(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ((MasterLeitorViewModel)BindingContext).ThisOnAppearingOpcoes();
            else
                ((MasterLeitorViewModel)BindingContext).SearchOpcoes(e.NewTextValue);
        }


        public async void List_OnSelectedOpcoes(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                menuSelecionado = true;

                var menuItemSelecionado = (ItensMenu)e.SelectedItem;

                if (menuItemSelecionado.id == 1)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ImagemLeitorPage(AppLeitor.Leitor.imgArqs));
                }
                else if (menuItemSelecionado.id == 2)
                {
                    await Application.Current.MainPage.Navigation.PushPopupAsync(new AudioPop(AppLeitor.Leitor.audioArqs));
                }   
                else if (menuItemSelecionado.id == 3)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new VideoLeitorPage(AppLeitor.Leitor.videoArqs));
                }
                else if (menuItemSelecionado.id == 4)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new GraficoLeitorPage(AppLeitor.Leitor.graficoArqs));
                }
                else if (menuItemSelecionado.id == 5)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new TabelaLeitorPage(AppLeitor.Leitor.tabelaArqs));
                }
                   
                OpcoesListView.SelectedItem = null;
            }
        }
    }
}

