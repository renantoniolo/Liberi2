using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using Lux.Helpers;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Xamarin.Forms;
using Lux.Views.Popup;

namespace Lux.ViewsModels
{
    public class ComentarioViewModel : BaseViewModel
    {
        private int id_livro;
        private IAlertService alertService;

        private double aval = 0;
        public double Aval
        {
            get { return aval; }
            set { SetProperty(ref aval, value); }
        }

        public ComentarioViewModel(int id_livro)
        {
            this.id_livro = id_livro;
            this.alertService = DependencyService.Get<IAlertService>();

            this.LoadComentarioCommand = new Command(async () => await LoadComentarioAsync(), () => !IsBusy);
            this.PostarComentarioCommand = new Command(async () => await PostComentarioAsync(), () => !IsBusy);
        }

        internal async Task ThisOnAppearing()
        {
            // carrega lista de coemntarios
            await LoadComentarioAsync();
        }

        private string inputComentario;
        public string InputComentario
        {
            get { return inputComentario; }
            set
            {
                inputComentario = value;
                this.Notify(nameof(InputComentario));
            }
        }

        private ObservableCollection<Comentario> comentarios;
        public ObservableCollection<Comentario> Comentarios
        {
            get { return comentarios; }
            set
            {
                comentarios = value;
                this.Notify(nameof(Comentarios));
            }
        }

        public ICommand LoadComentarioCommand { get; set; }

        public async Task LoadComentarioAsync()
        {
            try
            {
                IsBusy = true;
                ImagemConverter imagem = new ImagemConverter();

                var listComentarios = await RestService.GetAllComentarios(id_livro);

                foreach (Comentario c in listComentarios)
                {

                    if (!String.IsNullOrEmpty(c.foto))
                    {
                        if (c.cpf.Equals(App.user.Cpf))
                            c.isUser = true;

                        c.imageSource = imagem.GetImageBase64ToImageSource(c.foto);
                        c.DeleteCommand = new Command(async (object obj) => await Delete(obj));
                        c.HistoricoCommand = new Command(async (object obj) => await Historico(obj));
                        c.EditCommand = new Command(async (object obj) => await Edit(obj));
                    }
                    if (c.imageSource == null) c.imageSource = "avatar";
                }

                Comentarios = new ObservableCollection<Comentario>(listComentarios);
            }
			catch(Exception exception){
                Function.AppCenterCrashes(exception, "ComentarioViewModel", "LoadComentarioAsync()");
			}
			finally{
				IsBusy = false;
			}
		}

        public async Task Delete(object id)
        {
            long commentId = Convert.ToInt32(id);
            try
            {
                bool result = await alertService.ShowAsyncConfirm("Aviso", "Deseja realmente excluir o seu comentario?", "Sim", "Não");
                if (result)
                {
                    string resp = await RestService.DeleteComentario(commentId);
                    if (resp.Contains("sucesso"))
                    {
                        LoadComentarioAsync();
                        await alertService.ShowAsync("Sucesso", "Comentarioexcluido com sucesso", "Ok");
                    }
                    else await alertService.ShowAsync("Error", resp, "Ok");
                }
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Error", "Falha ao excluir seu comentario", "OK");
                Function.AppCenterCrashes(exception, "ComentarioViewModel", "Delete()");
            }
        }

        public async Task Edit(object obj)
        {
            Comentario commet = obj as Comentario;
            try
            {
                await App.NavigationPushPopup(new EditComentarioPop(commet, this));
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Error", "Falha ao abrir edição desse comentario", "OK");
                Function.AppCenterCrashes(exception, "ComentarioViewModel", "Edit()");
            }
        }

        public async Task Historico(object obj)
        {
            string cpf = obj as string;
            try
            {
                await App.NavigationPushPopup(new HistoricoComentarioPop(cpf,id_livro));
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Error", "Falha ao abrir historico desse comentario", "OK");
                Function.AppCenterCrashes(exception, "ComentarioViewModel", "Historico()");
            }
        }

        public ICommand PostarComentarioCommand { get; set; }

        public async Task PostComentarioAsync()
        {
            if (!String.IsNullOrWhiteSpace(InputComentario))
            {

                bool retorno = await RestService.PostComentario(id_livro, App.user, InputComentario, Aval);

                if (retorno)
                {
                    await alertService.ShowAsync("Atenção", "Comentário incluido com sucesso.", "Ok");
                    LoadComentarioAsync();
                }
                else await alertService.ShowAsync("Atenção", "Falha ao publicar o Comentário.", "Ok");
            }
            else await alertService.ShowAsync("Atenção", "Preencha o campo comentário.", "Ok");
        }
    }
}
