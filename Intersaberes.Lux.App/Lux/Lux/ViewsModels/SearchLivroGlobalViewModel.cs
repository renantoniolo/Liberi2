using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lux.Helpers;
using Lux.Infra;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Lux.Views;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class SearchLivroGlobalViewModel : BaseViewModel
    {

        public SearchLivroGlobalViewModel()
        {
            Items = new ObservableCollection<Livro>();

            //comands
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CloseModalCommand = new Command(async () => await CloseModalAsyncCommand());
            SearchCommand = new Command(async (object obj) => await SearchAsyncCommand(obj), (obj) => !IsBusy);
            //ItemTappedCommand = new Command((object obj) => ItemTappedAsyncCommand(obj), (obj) => !IsBusy);
        }


        public Command LoadItemsCommand { get; set; }
        public Command CloseModalCommand { get; set; }
        public Command SearchCommand { get; set; }

        private ObservableCollection<Livro> items;
        public ObservableCollection<Livro> Items
        {
            get { return items; }
            set
            {
                items = value;
                this.Notify(nameof(Items));
            }
        }

        private Livro itemSelecionado;
        public Livro ItemtSelecionado
        {
            get { return itemSelecionado; }
            set
            {
                if (!IsBusy)
                {
                    try
                    {
                        IsBusy = true;
                        var item = (Livro)value;

                        if (item != null)
                        {
                            App.NavigationPush(new LivroDetailsPage(item));

                            // reset tap
                            itemSelecionado = null;
                            Notify(nameof(ItemtSelecionado));
                        }

                    }
                    catch (Exception exception)
                    {
                        Function.AppCenterCrashes(exception, "SearchLivroGlobalViewModel", "ItemSelecionado");
                    }
                    finally
                    {
                        IsBusy = false;
                        this.CloseModalAsyncCommand();
                    }
                }
            }
        }

        internal void ThisOnAppearing() => ExecuteLoadItemsCommand();


        async Task SearchAsyncCommand(object x)
        {
            try
            {
                var text = (string)x;

                if (string.IsNullOrWhiteSpace(text))
                    this.LoadItemsCommand.Execute(null);
                else
                    await this.ItensChanges(text);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "SearchLivroGlobalViewModel", "SearchAsyncCommand()");
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();

                Items.Clear();

                List<Livro> livros = new List<Livro>();

                var realm = new RealmBase();

                ObservableCollection<LivroBaixado> livrosBaixados = realm.GetByLivroBaixadoAll();//db.GetAll();

                List<Livro> listLivrosApi = await RestService.GetAllLivros();


                foreach (Livro l in listLivrosApi)
                {
                    l.isBuy = "NÃO";
                    foreach (LivroBaixado lb in livrosBaixados)
                    {
                        if (l.Id == lb.Id) l.isBuy = "SIM";
                        else l.isBuy = "NÃO";
                    }
                    livros.Add(l);
                }

                Items = new ObservableCollection<Livro>(livros);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "SearchLivroGlobalViewModel", "ExecuteLoadItemsCommand()");
            }
            finally
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
                IsBusy = false;
            }
        }

        public async Task ItensChanges(string textoSearch)
        {
            try
            {
                var list = Items.Where(i => i.Titulo.ToLower().Contains(textoSearch.ToLower())).ToList();

                Items.Clear();
                Items = new ObservableCollection<Livro>(list);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "SearchLivroGlobalViewModel", "ItensChanges()");
            }
        }

        private  async Task CloseModalAsyncCommand()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PopModalAsync(true);
            }
            catch(Exception exception)
            {
                Function.AppCenterCrashes(exception, "SearchLivroGlobalViewModel", "CloseModalAsyncCommand()");
            }
        }


    }
}
