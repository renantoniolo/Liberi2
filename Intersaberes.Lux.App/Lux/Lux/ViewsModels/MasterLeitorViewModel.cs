using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Lux.Models;
using Xamarin.Forms;
using Lux.Views;
using Lux.Helpers;
using Lux.Interfaces;

namespace Lux.ViewsModels
{
    public class MasterLeitorViewModel : BaseViewModel
    {
        public ObservableCollection<ItensMenu> Caps { get; set; }
        public ObservableCollection<ItensMenu> Opcoes { get; set; }

        public Command LoadCapsCommand { get; set; }
        public Command LoadOpcoesCommand { get; set; }
        public Command VoltarCommand { get; set; }

        private Livro livro;

        public MasterLeitorViewModel(Livro livro)
        {
            this.livro = livro;
            Caps = new ObservableCollection<ItensMenu>();
            Opcoes = new ObservableCollection<ItensMenu>();

            LoadOpcoesCommand = new Command(async () => await ExecuteLoadOpcoesCommand());
            VoltarCommand = new Command(async () => await Voltar());
        }


        internal void ThisOnAppearingOpcoes() => ExecuteLoadOpcoesCommand();

     
        async Task ExecuteLoadOpcoesCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Opcoes.Clear();

                if (AppLeitor.Leitor.imgArqs.Count > 0)
                {
                    Opcoes.Add(new ItensMenu() { lblView = "Imagens", backgroundColor = "#686868", id = 1 });
                }
                if (AppLeitor.Leitor.audioArqs.Count > 0)
                {
                    Opcoes.Add(new ItensMenu() { lblView = "Áudios", backgroundColor = "#686868", id = 2 });
                }
                if (AppLeitor.Leitor.videoArqs.Count > 0)
                {
                    Opcoes.Add(new ItensMenu() { lblView = "Vídeos", backgroundColor = "#686868", id = 3 });
                }
                if (AppLeitor.Leitor.graficoArqs.Count > 0)
                {
                    Opcoes.Add(new ItensMenu() { lblView = "Graficos", backgroundColor = "#686868", id = 4 });
                }
                if (AppLeitor.Leitor.tabelaArqs.Count > 0)
                {
                    Opcoes.Add(new ItensMenu() { lblView = "Tabelas", backgroundColor = "#686868", id = 5 });
                }

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MasterLeitorViewModel", "ExecuteLoadOpcoesCommand()");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task Voltar()
        {
            App.ePUBViewModel.PercentualLeitura();
            await Application.Current.MainPage.Navigation.PopAsync();

        }

        public void SearchOpcoes(string textoSearch)
        {
            var listAux = Opcoes.Where(i => Helpers.Function.removerAcentos(i.lblView.ToLower()).Contains(Helpers.Function.removerAcentos(textoSearch.ToLower()))).ToList();
            Opcoes.Clear();
            Opcoes = new ObservableCollection<ItensMenu>(listAux);
            OnPropertyChanged("Opcoes");
        }
    }
}
