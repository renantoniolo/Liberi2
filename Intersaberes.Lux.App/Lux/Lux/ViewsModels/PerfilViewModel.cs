using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Lux.Helpers;
using Lux.Infra;
using Lux.Interfaces;
using Lux.Models;
using Lux.Services;
using Lux.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Realms;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class PerfilViewModel : BaseViewModel
    {
        private IAlertService alertService;
        private string imgBase64;
        private MasterViewModel masterViewModel;

        public ICommand CameraCommand { get; set; }
        public ICommand SenhaCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public PerfilViewModel(MasterViewModel masterViewModel)
        {
            BoolIconLabel = true;
            BoolImageSource = false;
            this.masterViewModel = masterViewModel;

            this.alertService = DependencyService.Get<IAlertService>();

            this.CameraCommand = new Command(async () => await CameraAsync(), () => !IsBusy);
            this.SaveCommand = new Command(async () => await SalvarAsync(), () => !IsBusy);
            this.SenhaCommand = new Command(async () => await SenhaAsync(), () => !IsBusy);

            // carrega valores
            this.LoadValues();
        }

        internal async Task ThisOnAppearing()
        {

        }

        private async void LoadValues(){

            try
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();

                if (App.user != null)
                {
                    InputNome = App.user.Nome;
                    InputEmail = App.user.Email;
                    InputCidade = App.user.Cidade;

                    if (!String.IsNullOrEmpty(App.user.Foto))
                    {
                        ImagemConverter imagem = new ImagemConverter();
                        ImageSource = imagem.GetImageBase64ToImageSource(App.user.Foto);
                        imgBase64 = App.user.Foto;
                        BoolIconLabel = false;
                        BoolImageSource = true;
                    }
                }

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "PerfilViewModel", "ThisOnAppearing()");
            }
            finally
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
            }


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

        private string inputCidade;
        public string InputCidade
        {
            get { return inputCidade; }
            set
            {
                inputCidade = value;

                this.Notify(nameof(InputCidade));
            }
        }

        private ImageSource profileImage;
        public ImageSource ImageSource
        {
            get
            {
                return profileImage;
            }
            set
            {
                profileImage = value;
                Notify(nameof(ImageSource));
            }
        }

        private bool boolImageSource;
        public bool BoolImageSource
        {
            get
            {
                return boolImageSource;
            }
            set
            {
                boolImageSource = value;
                Notify(nameof(BoolImageSource));
            }
        }

        private bool boolIconLabel;
        public bool BoolIconLabel
        {
            get
            {
                return boolIconLabel;
            }
            set
            {
                boolIconLabel = value;
                Notify(nameof(BoolIconLabel));
            }
        }

        private async Task SenhaAsync()
        {
            await App.NavigationPush(new TrocarSenhaPage(App.user));
        }

        private async Task CameraAsync()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        CompressionQuality = 99,
                        PhotoSize = PhotoSize.Small,
                    });

                    if (file != null)
                    {
                        ImagemConverter imagem = new ImagemConverter();
                        imgBase64 = await imagem.Convertto64(file);
                        ImageSource = file.Path;

                        BoolIconLabel = false;
                        BoolImageSource = true;
                    }
                }
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Atenção", "Falha ao carregar a imagem do seu perfil.", "Ok");
                Function.AppCenterCrashes(exception, "PerfilViewModel", "CameraAsync()");
            }
        }

        private async Task SalvarAsync()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;

                    if (await VerificaCampos())
                    {
                        await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();


                        var realmBase = new RealmBase();
                        realmBase.UpdatePerfilUsuario(InputNome, InputEmail, InputCidade, imgBase64);

                        // envia o cadastro do usuário para webApi
                        bool resp = await RestService.PutUsuario(App.user);
                        if (!resp) Debug.WriteLine("Falha ao enviar Perfil para webApi.");

                        // para o loader
                        await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();

                        await alertService.ShowAsync("Informativo", "Usuário atualizado com sucesso!", "Ok");
                        masterViewModel.LoadPerfil();
                    }

                }
                catch (Exception exception)
                {
                    await alertService.ShowAsync("Atenção", "Falha ao atualizar as informações do usuário!", "Ok");
                    Function.AppCenterCrashes(exception, "PerfilViewModel", "SalvarAsync()");
                }
                finally
                {
                    await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
                    this.LoadValues();
                    IsBusy = false;
                }
            }
        }

        private async Task<bool> VerificaCampos(){

            if(String.IsNullOrEmpty(InputNome)) { await alertService.ShowAsync("Atenção", "Informe um Nome!", "Ok"); return false; }
            if(String.IsNullOrEmpty(InputEmail)) { await alertService.ShowAsync("Atenção", "Informe um E-mail!", "Ok"); return false; }
            if(String.IsNullOrEmpty(InputCidade)) { await alertService.ShowAsync("Atenção", "Informe uma Cidade!", "Ok"); return false; }

            return true;
        }
    }
}
