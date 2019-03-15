using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

using Lux.Views;
using Lux.Models;
using Lux.Services;
using Lux.Interfaces;

using Xamarin.Forms;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Lux.Helpers;
using Plugin.Connectivity;
using System.Collections.ObjectModel;

namespace Lux.ViewsModels
{
    class HomeViewModel : BaseViewModel
    {
        private IAlertService alertService;
        public Livro livro0 { get; set; }
        public Livro livro1 { get; set; }
        public Livro livro2 { get; set; }

        public HomeViewModel()
        {
            this.alertService = DependencyService.Get<IAlertService>();

            this.LoadCommand = new Command(async () => await ThisOnAppearing());
            this.BibliotecaCommand = new Command(async () => await OpenToBibliotecaAsync());
            this.LojaCommand = new Command(async () => await OpenToLojaAsync());
            this.DetalhesCommand = new Command(OpenToDetalhesAsync);
            this.SearchGlobalCommand = new Command(async () => await OpenSearchModalToAsync());
        }

        async Task ThisOnAppearing()
        {

            try
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();
                var livros = await RestService.GetAllLivros();

                livros =  livros.OrderByDescending(p => p.Dta_UltimaLeitura).ToList();


                for (int i = 0; i < livros.Count; i++){

                    if(i == 0) livro0 = livros[i];
                    if(i == 1) livro1 = livros[i];
                    if(i == 2) livro2 = livros[i];
                }

                if(livro0.Dta_UltimaLeitura == null) TextDetalhe = "Nossa sugestão para você iniciar uma leitura.";
                else TextDetalhe = "Continue lendo o livro que você parou!";

                if (livros.Count > 0){
                    // se vier pelo menos um livro deixa tudo igual
                    if(livro1 == null) livro1 = livros[0];
                    if(livro2 == null) livro2 = livros[0];
                }

                OnPropertyChanged("livro0");
                OnPropertyChanged("livro1");
                OnPropertyChanged("livro2");
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "HomePageViewModel", "ThisOnAppearing()");
            }
            finally
            {
                IsBusy = false;
                // atualiza o percentual de leitura
                await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();

            }
        }

        //Variavle Binding
        public ConfigUsuario config { get; set; }

        //Comands
        public ICommand LoadCommand { get; set; }

        public ICommand SearchGlobalCommand { get; set; }

        public ICommand BibliotecaCommand { get; set; }


        private string textDetalhe;
        public string TextDetalhe
        {
            get { return textDetalhe; }
            set
            {
                textDetalhe = value;
                this.Notify(nameof(TextDetalhe));
            }
        }

        private async Task OpenSearchModalToAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await App.NavigationPushModal(new SearchLivroGlobalPage());
                IsBusy = false;
            }
        }

        public async Task OpenToBibliotecaAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await App.NavigationPush(new BibliotecaPage());
                IsBusy = false;
            }
        }

        public ICommand LojaCommand { get; set; }

        public async Task OpenToLojaAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    Device.OpenUri(new Uri("https://www.livrariaintersaberes.com.br"));
                }
                catch(Exception exception)
                {
                    Function.AppCenterCrashes(exception, "HomePageViewModel", "OpenToLojaAsync()");
                }
                finally{
                    IsBusy = false;
                }
            }
        }

        public ICommand DetalhesCommand { get; set; }

        public void OpenToDetalhesAsync(object Livro)
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;
                    var livro = Livro as Livro;
                    App.NavigationPush(new LivroDetailsPage(livro));
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "HomePageViewModel", "OpenToDetalhesAsync");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
