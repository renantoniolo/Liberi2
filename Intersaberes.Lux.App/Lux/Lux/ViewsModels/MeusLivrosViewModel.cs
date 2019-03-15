using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Lux.Helpers;
using Lux.Models;
using Lux.Services;
using Lux.Views;
using Xamarin.Forms;
using Lux.Interfaces;
using Lux.Infra;

namespace Lux.ViewsModels
{
    public class MeusLivrosViewModel : BaseViewModel
    {

        public MeusLivrosViewModel()
        {
            Title = "Meus Livros";
            Items = new ObservableCollection<Livro>();

            // commands
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ViewBooksListCommand = new Command(() => ExecuteViewBooksList());
            ViewBooksGridCommand = new Command(() => ExecuteViewBooksGrid());
            SearchCommand = new Command(async (object obj) => await SearchAsyncCommand(obj), (obj) => !IsBusy);
            ItemTappedCommand = new Command((object obj) => ItemTappedAsyncCommand(obj), (obj) => !IsBusy);

            IsViewList = true;
            IsViewGrid = false;
        }

        private bool isViewList;
        public bool IsViewList
        {
            get { return isViewList; }
            set
            {
                isViewList = value;
                this.Notify(nameof(IsViewList));
            }
        }

        private bool isViewGrid;
        public bool IsViewGrid
        {
            get { return isViewGrid; }
            set
            {
                isViewGrid = value;
                this.Notify(nameof(IsViewGrid));
            }
        }

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
                        Function.AppCenterCrashes(exception, "MeusLivrosViewModel", "ItemSelecionado");
                    }
                    finally{
                        IsBusy = false;
                    }
                }
            }
        }

        public Command ItemTappedCommand { get; set; }

        void ItemTappedAsyncCommand(object x)
        {

            try
            {
                var item = (Livro)x;

                if (item != null)
                {
                    App.NavigationPush(new LivroDetailsPage(item));
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MeusLivrosViewModel", "ItemTappedAsyncCommand()");
            }
        }

        internal void ThisOnAppearing() => ExecuteLoadItemsCommand();

        public Command LoadItemsCommand { get; set; }

        public Command ViewBooksListCommand { get; set; }

        public Command ViewBooksGridCommand { get; set; }

        public Command SearchCommand { get; set; }
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
                Function.AppCenterCrashes(exception, "MeusLivrosViewModel", "SearchAsyncCommand()");
            }
        }

        private void ExecuteViewBooksList()
        {
            IsViewList = true;
            IsViewGrid = false;
        }

        private void ExecuteViewBooksGrid()
        {
            IsViewGrid = true;
            IsViewList = false;
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();

                Items.Clear();

                List<Livro> livros = new List<Livro>();

                var realm = new RealmBase();

                ObservableCollection<LivroBaixado> livrosBaixados = realm.GetByLivroBaixadoAll();

                List<Livro> listLivrosApi = await RestService.GetAllLivros();

                foreach (Livro l in listLivrosApi)
                {

                    foreach (LivroBaixado lb in livrosBaixados)
                    {

                        if (l.Id == lb.Id)
                        {
                            livros.Add(l);
                        }
                    }
                }

                Items = new ObservableCollection<Livro>(livros);
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MeusLivrosViewModel", "ExecuteLoadItemsCommand()");
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
                Function.AppCenterCrashes(exception, "MeusLivrosViewModel", "ItensChanges()");
            }
        }
    }

}
