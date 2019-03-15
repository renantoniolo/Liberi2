using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Lux.Helpers;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class TrocarSenhaViewModel : BaseViewModel
    {
        private IAlertService alertService;

        private Usuario usuarioLogado;

        public ICommand SaveCommand { get; set; }

        public TrocarSenhaViewModel(Usuario usuario)
        {
            this.usuarioLogado = usuario;

            this.alertService = DependencyService.Get<IAlertService>();
            this.SaveCommand = new Command(async () => await SalvarAsync(), () => !IsBusy);
        }

        private string inputSenhaAnterior;
        public string InputSenhaAnterior
        {
            get { return inputSenhaAnterior; }
            set
            {
                inputSenhaAnterior = value;

                this.Notify(nameof(InputSenhaAnterior));
            }
        }

        private string inputNovaSenha;
        public string InputNovaSenha
        {
            get { return inputNovaSenha; }
            set
            {
                inputNovaSenha = value;

                this.Notify(nameof(InputNovaSenha));
            }
        }

        private string inputRepetirSenha;
        public string InputRepetirSenha
        {
            get { return inputRepetirSenha; }
            set
            {
                inputRepetirSenha = value;

                this.Notify(nameof(InputRepetirSenha));
            }
        }

        private async Task SalvarAsync()
        {
            try
            {
                IsBusy = true;

                if (await VerificaCampos())
                {
                    UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);

                    string srt_retorno = await RestService.TrocarSenha(usuarioLogado.Email, InputSenhaAnterior, InputNovaSenha);

                    UserDialogs.Instance.HideLoading();

                    await alertService.ShowAsync("Informativo", srt_retorno, "Ok");

                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "TrocarSenhaViewModel", "SalvarAsync()");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> VerificaCampos()
        {

            if (String.IsNullOrEmpty(InputSenhaAnterior)) { await alertService.ShowAsync("Atenção", "Informe a senha anterior!", "Ok"); return false; }
            if (String.IsNullOrEmpty(InputNovaSenha)) { await alertService.ShowAsync("Atenção", "Informe uma nova senha!", "Ok"); return false; }
            if (InputNovaSenha != InputRepetirSenha) { await alertService.ShowAsync("Atenção", "Senhas não conferem!", "Ok"); return false; }

            return true;
        }

    }
}
