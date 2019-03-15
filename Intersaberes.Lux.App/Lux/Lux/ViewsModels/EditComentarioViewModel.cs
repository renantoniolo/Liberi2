using System;
using System.Windows.Input;
using System.Threading.Tasks;

using Lux.Models;
using Lux.Services;
using Lux.Interfaces;
using Xamarin.Forms;
using Lux.Helpers;

namespace Lux.ViewsModels
{
    public class EditComentarioViewModel : BaseViewModel
    {
        private IAlertService alertService;
        ComentarioViewModel comentarioViewModel;
        public ICommand SalveCommand {get;set;}

        private Comentario comentario;
        public Comentario Comentario
        {
            get { return comentario; }
            set { SetProperty(ref comentario, value); }
        }

        private double aval = 0;
        public double Aval
        {
            get { return aval; }
            set { SetProperty(ref aval, value); }
        }

        public EditComentarioViewModel(Comentario comentario, ComentarioViewModel comentarioViewModel)
        {
            this.comentarioViewModel = comentarioViewModel;
			this.Comentario = comentario;
            Aval = comentario.avaliacao;

            this.alertService = DependencyService.Get<IAlertService>();
            SalveCommand = new Command(async () => await Salve());
        }

        async Task Salve()
        {
            try
            {
                bool resp = await RestService.UpdateComentario(Comentario.Id.ToString(), Comentario._Comentario, Aval);
                if (resp)
                {
                    comentarioViewModel.ThisOnAppearing();
                    await alertService.ShowAsync("Sucesso", "Seu comentario foi atualizado", "Ok");
                    await App.NavigationPopPopup();
                }
                else
                {
                    await alertService.ShowAsync("Error", "Falha ao atualizar seu comentario", "Ok");
                }
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Error", "Falha ao atualizar seu comentario", "Ok");

                Function.AppCenterCrashes(exception, "EditComentarioViewModel", "Salve()");
            }
        }
    }
}
