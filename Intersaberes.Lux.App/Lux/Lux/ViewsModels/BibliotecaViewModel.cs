using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Lux.Views;
using Lux.Models;
using Lux.Services;

using Xamarin.Forms;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Lux.Helpers;

namespace Lux.ViewsModels
{
    public class BibliotecaViewModel : BaseViewModel
    {
        public BibliotecaViewModel()
        {
            Title = "Biblioteca";
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
                try
                {
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
                    Function.AppCenterCrashes(exception, "BibliotecaViewModel", "ItemSelecionado");
                }
            }
        }
        
		public Command ItemTappedCommand { get; set; }

		void ItemTappedAsyncCommand(object x){

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
                Function.AppCenterCrashes(exception, "BibliotecaViewModel", "ItemTappedAsyncCommand()");
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
                Function.AppCenterCrashes(exception, "BibliotecaViewModel", "SearchAsyncCommand()");
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
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                Items.Clear();
                Items = new ObservableCollection<Livro>(await RestService.GetAllLivros());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
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
                Function.AppCenterCrashes(exception, "BibliotecaViewModel", "ItensChanges()");
            }
        }
    }
}

