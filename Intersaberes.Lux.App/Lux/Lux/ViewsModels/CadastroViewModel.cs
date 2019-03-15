using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Lux.Helpers;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Lux.Views;
using Lux.Views.Popup;
using Microsoft.AppCenter.Crashes;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class CadastroViewModel : BaseViewModel
    {
        private IAlertService alertService;

        public ICommand LoginCommand { get; set; }
        public ICommand OkCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand TermosUsoCommand { get; set; }

        public CadastroViewModel() 
        {
            this.alertService = DependencyService.Get<IAlertService>();
            this.LoginCommand = new Command(() => LoginAsync(), () => !IsBusy);
            this.OkCommand = new Command(async () => await OkAsync(), () => !IsBusy);
            this.TermosUsoCommand = new Command(async () => await TermosUsoAsync(), () => !IsBusy);
            this.ClosePopupCommand = new Command(async () => await ClosePopupAsync(), () => !IsBusy);
        }

        private string inputNome;
        public string InputNome
        {
            get { return inputNome; }
            set
            {
                inputNome = value;
                this.Notify(nameof(InputNome));
            }
        }

        private string inputCpf;
        public string InputCpf
        {
            get { return inputCpf; }
            set
            {
                inputCpf = value;
                this.Notify(nameof(InputCpf));
            }
        }

        private string inputEmail;
        public string InputEmail
        {
            get { return inputEmail; }
            set
            {
                inputEmail = value;
                this.Notify(nameof(InputEmail));
            }
        }

        private string inputSenha;
        public string InputSenha
        {
            get { return inputSenha; }
            set
            {
                inputSenha = value;
                this.Notify(nameof(InputSenha));
            }
        }

        private string inputConfSenha;
        public string InputConfSenha
        {
            get { return inputConfSenha; }
            set
            {
                inputConfSenha = value;
                this.Notify(nameof(InputConfSenha));
            }
        }

        private async Task OkAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

                    if (await VerificaCampos())
                    {
                        UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);

                        // remove os caracteres do cpf
                        string cpf = InputCpf.Replace(".", "");
                        cpf = cpf.Replace("-", "");

                        Usuario usuario = new Usuario();
                        usuario.Nome = InputNome;
                        usuario.Email = InputEmail;
                        usuario.Cpf = cpf;
                        usuario.Senha = InputSenha;

                        // envia o cadastro do usuário para webApi
                        string strResp = await RestService.PostUsuario(usuario);

                        if (strResp == null) {
                            await alertService.ShowAsync("Atenção", "Falha ao cadastrar.", "Ok");
                        }
                        else{
                            await alertService.ShowAsync("Informativo", strResp, "Ok");
                            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                        }

                    }
                }
                catch (Exception exception)
                {
                    Function.AppCenterCrashes(exception, "CadastroViewModel", "OkAsync()");
                }
                finally
                {
                    UserDialogs.Instance.HideLoading();
                    IsBusy = false;
                }
            }
        }

        private async Task<bool> VerificaCampos(){

            if (String.IsNullOrEmpty(InputNome)) { await alertService.ShowAsync("Atenção", "Informe um Nome!", "Ok"); return false; }
            if (String.IsNullOrEmpty(InputCpf)) { await alertService.ShowAsync("Atenção", "Informe um CPF!", "Ok"); return false; }
            if (String.IsNullOrEmpty(InputEmail)) { await alertService.ShowAsync("Atenção", "Informe um E-mail!", "Ok"); return false; }
            if (String.IsNullOrEmpty(InputSenha)) { await alertService.ShowAsync("Atenção", "Informe uma Senha!", "Ok"); return false; }
            if (InputSenha != inputConfSenha) { await alertService.ShowAsync("Atenção", "Senhas não conferem!", "Ok"); return false; }

            return true;
        }

        private async Task TermosUsoAsync()
        {
            if (!IsBusy)
            {
                var popup = new TermosUsoPop();
                await Application.Current.MainPage.Navigation.PushPopupAsync(popup);
                IsBusy = false;
            }
        }

        private async Task ClosePopupAsync()
        {
            await App.NavigateToClosePopup();
        }

        private async Task LoginAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());

                IsBusy = false;
            }
        }
    }
}
