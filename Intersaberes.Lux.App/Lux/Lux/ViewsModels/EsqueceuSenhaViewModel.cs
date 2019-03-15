using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Lux.Helpers;
using Lux.Interfaces;
using Lux.Services;
using Lux.Views;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class EsqueceuSenhaViewModel : BaseViewModel
    {

        private IAlertService alertService;

        public ICommand LoginCommand { get; set; }
        public ICommand OkCommand { get; set; }

        public EsqueceuSenhaViewModel()
        {
            this.alertService = DependencyService.Get<IAlertService>();
            this.OkCommand = new Command(async () => await OkAsync(), () => !IsBusy);
            this.LoginCommand = new Command(async() => await LoginAsync(), () => !IsBusy);
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

        private async Task LoginAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                }
                catch (Exception exception)
                {
                    Function.AppCenterCrashes(exception, "EsqueceuSenhaViewModel", "LoginAsync()");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private async Task OkAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

                    if (!String.IsNullOrEmpty(InputEmail)) {

                        UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);
                        
                        // envia o cadastro do usuário para webApi
                        string resp = await RestService.PostRecuperarSenha(InputEmail);

                        // exibe mensagem
						await alertService.ShowAsync("Informativo", resp, "Ok");
                      
                    }
                    else{ 
                        await alertService.ShowAsync("Atenção", "Informe um e-mail!", "Ok"); 
                    }

                }
                catch (Exception exception)
                {
                    Function.AppCenterCrashes(exception, "EsqueceuSenhaViewModel", "OkAsync()");
                }
                finally{
                    UserDialogs.Instance.HideLoading();
                    IsBusy = false;
                }
            }
        }
    }
}
