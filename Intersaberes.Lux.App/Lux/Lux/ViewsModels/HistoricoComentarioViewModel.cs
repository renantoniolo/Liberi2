using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using Lux.Models;
using Lux.Services;
using Lux.Helpers;

namespace Lux.ViewsModels
{
	public class HistoricoComentarioViewModel : BaseViewModel
    {
        private long id_livro;
        private string cpf;

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

        public HistoricoComentarioViewModel(string cpf,long id_livro)
        {
			this.id_livro = id_livro;
            this.cpf = cpf;
        }
        
		internal async Task ThisOnAppearing()
		{
            // carrega lista de historico de comentarios
			await LoadComentarioAsync();
		}

		public ICommand LoadComentarioCommand { get; set; }

		public async Task LoadComentarioAsync()
		{
			try
			{
				IsBusy = true;
                ImagemConverter imagem = new ImagemConverter();

				var listComentarios = await RestService.GetHistoricoComentario(cpf, id_livro);

                foreach(Comentario c in listComentarios){

                    if (!String.IsNullOrEmpty(c.foto))
                    {
                        if (c.cpf.Equals(App.user.Cpf))
                            c.isUser = true;

                        c.imageSource = imagem.GetImageBase64ToImageSource(c.foto);
                    }
                }

				Comentarios = new ObservableCollection<Comentario>(listComentarios);

			}catch(Exception ex){

				Debug.WriteLine("Erro ao carrega da webApi" + ex.Message);
			}
			finally{
				IsBusy = false;
			}
		}
    }
}
