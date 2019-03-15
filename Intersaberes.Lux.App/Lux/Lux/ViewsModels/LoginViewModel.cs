using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

using Lux.Views;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;

using Xamarin.Forms;
using Acr.UserDialogs;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Lux.Helpers;
using Microsoft.AppCenter.Push;
using Lux.Infra;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Realms;

namespace Lux.ViewsModels
{
    public class LoginViewModel : BaseViewModel
    {
        private IAlertService alertService;

        private string inputUser;
        public string InputUser
        {
            get { return inputUser; }
            set
            {
                inputUser = value;
                this.Notify(nameof(InputUser));
            }
        }

        private string inputPass;
        public string InputPass
        {
            get { return inputPass; }
            set
            {
                inputPass = value;
                this.Notify(nameof(InputPass));
            }
        }

        private bool btnEntrar;
        public bool BtnEntrar
        {

            get { return btnEntrar; }
            set
            {
                btnEntrar = value;
                this.Notify(nameof(BtnEntrar));
            }
        }

        //Comands
        public ICommand EsqueceuSenhaCommand { get; set; }
        public ICommand CadastroCommand { get; set; }
        public ICommand LoginCommand { get; set; }

     

        public LoginViewModel()
        {
            //InputUser = "renantoniolo@gmail.com";
            //InputPass = "teste123";

            this.alertService = DependencyService.Get<IAlertService>();

            BtnEntrar = true;

            this.EsqueceuSenhaCommand = new Command(async () => await EsqueceuSenhaAsync(), () => !IsBusy);
            this.LoginCommand = new Command(async () => await LoginAsync(), () => !IsBusy);
            this.CadastroCommand = new Command(async () => await CadastroAsync(), () => !IsBusy);
        }

        internal async Task ThisOnAppearing()
        {
            // habilita o push notifiction
            Push.SetEnabledAsync(true);

            bool isEnabled = await Push.IsEnabledAsync();

            if(!isEnabled){
                Function.AppCenterCrashes(null, "NOTIFICACAO APPCENTER", "NOTIFICACAO DESABILITADA NO MOBILE CENTER.");
            }

        }


        public async Task LoginAsync()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    BtnEntrar = false;

                    if (string.IsNullOrEmpty(InputUser) || string.IsNullOrEmpty(InputPass))
                    {
                        await alertService.ShowAsync("Error", "Preencha os Campos Login e Senha", "Ok");
                        BtnEntrar = true;
                    }
                    else
                    {
                        // pede as permissoes
                        PermissionService permissionService = new PermissionService();
                        await permissionService.startPermissionService();

                        UserDialogs.Instance.ShowLoading("Loading", MaskType.Gradient);

                        // verifica na webApi
                        LoginModel login = await RestService.Login(InputUser, InputPass);

                        if (login == null) await alertService.ShowAsync("Error", "Servidor não encontrado", "Ok");
                        else if (login.user == null)
                            await alertService.ShowAsync("Atenção", login.mensagem, "Ok");
                        else
                        {
                            App.token = login.token;
                            App.user = login.user;

                            App.Current.Properties["UserCPF"] = login.user.Cpf;
                            App.Current.Properties["DateIsLogado"] = DateTime.Now;
                            await App.Current.SavePropertiesAsync();

                            // faz isso, apenas uma vez por causa da modificacao na base de dados
                            if (!App.Current.Properties.ContainsKey("CreateNewBD"))
                            {
                                // Marca que ja foi criada uma base de dados nova
                                App.Current.Properties["CreateNewBD"] = "true";
                                await App.Current.SavePropertiesAsync();
                            }

                            InsertOrUpdateUsuario();
                            InsertOrUpdateConfig();

                            await Application.Current.MainPage.Navigation.PushAsync(new MainMasterDetailPage());
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Atenção", "Erro ao logar: " + exception.Message, "OK");
                Function.AppCenterCrashes(exception, "LoginViewModel", "LoginAsync()");
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
                BtnEntrar = true;
                IsBusy = false;
            }
        }
        private async Task CadastroAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                await Application.Current.MainPage.Navigation.PushAsync(new CadastroPage());

                IsBusy = false;
            }

        }
        public async Task EsqueceuSenhaAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;

                //App.Current.MainPage = new EsqueceuSenhaPage();
                await Application.Current.MainPage.Navigation.PushAsync(new EsqueceuSenhaPage());

                IsBusy = false;
            }
        }

        //Banco
        void InsertOrUpdateUsuario()
        {
            App.user.DtUltimoLoginApp = DateTime.Now.ToString();
            App.user.configID = App.user.Id;

            var realm = new RealmBase();
            realm.InsertUpdate(App.user);
        }

        void InsertOrUpdateConfig()
        {
            var realm = new RealmBase();
            var conf = realm.GetByIdConfUsuario((int)App.user.Id);

            if (conf == null) // ja tinha salva essas configuracoes?
            {
                ConfigUsuario config = new ConfigUsuario();
                config.Id = (int)App.user.Id; // o id de cofiguracao é o mesmo do usuário perfil
                config.ImageDownload = true;
                config.SoundDownload = true;
                config.VideoDownload = true;

                realm.InsertUpdate(config);
            }

        }
    }
}
