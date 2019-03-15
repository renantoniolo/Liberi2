using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

using Lux.Views;
using Lux.Models;
using Lux.Helpers;
using Lux.Services;
using Lux.Interfaces;

using Xamarin.Forms;
using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Lux.Views.Animation;
using System.IO.Compression;
using AppLeitor;
using System.Collections.Generic;
using Lux.Infra;

namespace Lux.ViewsModels
{
    public class LivroDetailsViewModel : BaseViewModel
    {
        private IAlertService alertService;

        public ICommand ConfigCommand { get; set; }
        public ICommand DownloadBookCommand { get; set; }
        public ICommand AmostraBookCommand { get; set; }
        public ICommand ComentarioCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand LerCommand { get; set; }
        public ICommand ImageCapaCommand { get; set; }

        private bool isBaixar;
        public bool IsBaixar
        {
            get
            {
                return isBaixar;
            }
            set
            {
                isBaixar = value;
                Notify(nameof(IsBaixar));
            }
        }

        private bool isVisibleComentario = false;
        public bool IsVisibleComentario
        {
            get
            {
                return isVisibleComentario;
            }
            set
            {
                isVisibleComentario = value;
                Notify(nameof(IsVisibleComentario));
            }
        }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                imageSource = value;
                Notify(nameof(ImageSource));
            }
        }

        private Comentario comentario;
        public Comentario Comentario
        {
            get { return comentario; }
            set
            {
                comentario = value;
                this.Notify(nameof(Comentario));
            }
        }

        private Livro item;
        public Livro Item
        {
            get { return item; }
            set
            {
                item = value;
                this.Notify(nameof(Item));
            }
        }

        public LivroDetailsViewModel(Livro item = null)
        {
            Title = "Detalhes";
            Item = item;

            this.VerificBoookDownload();

            this.alertService = DependencyService.Get<IAlertService>();

            this.DownloadBookCommand = new Command(async () => await OpenToDownloadBookAsync(), () => !IsBusy);
            this.AmostraBookCommand = new Command(async () => await OpenToAmostradBookAsync(), () => !IsBusy);
            this.ConfigCommand = new Command(async () => await OpenToConfigAsync(), () => !IsBusy);
            this.ComentarioCommand = new Command(async () => await ComentarioAsync(), () => !IsBusy);
            this.DeleteCommand = new Command(async () => await DeleteAsync(), () => !IsBusy);
            this.LerCommand = new Command(async () => await LerAsync(), () => !IsBusy);
            this.ImageCapaCommand = new Command(async () => await ImageCapaAsync(), () => !IsBusy);
        }

        private void VerificBoookDownload(){

            var realm = new RealmBase();
            var livroCrypto = realm.GetByIdLivroBaixado((int)item.Id);
            if (livroCrypto == null) IsBaixar = true;
            else IsBaixar = false;

        }

        internal async Task ThisOnAppearing()
        {
            try
            {
                Comentario = await RestService.GetComentario((int)Item.Id);

                Analytics.TrackEvent("Acessando o Detalhes do Livro " + Item.Titulo);

                if (comentario != null)
                {

                    IsVisibleComentario = true;

                    if (!String.IsNullOrEmpty(Comentario.foto))
                    {
                        ImagemConverter imagem = new ImagemConverter();
                        ImageSource = imagem.GetImageBase64ToImageSource(Comentario.foto);
                    }
                    else ImageSource = "avatar";

                }
                else IsVisibleComentario = false;

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "LivroDetailsViewModel", "ThisOnAppearing()");
            }
        }

        //OpenPage
        public async Task ComentarioAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await App.NavigationPush(new ComentarioPage((int)Item.Id));
                IsBusy = false;
            }
        }

        private async Task ImageCapaAsync()
        {
            if (!IsBusy)
            {
                if(IsBaixar){
                    await OpenToDownloadBookAsync();
                }
                else{
                    await LerAsync();
                }
            }
        }

        public async Task OpenToAmostradBookAsync()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    if (Item.PrecoDigital != null)
                    {

                        Analytics.TrackEvent("Abrindo Amostra do Livro " + Item.Titulo);

                        await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();

                        AppLeitor.Leitor.isAmostra = false;
                        string isImage = string.Empty;
                        string isVideo = string.Empty;
                        string isAudio = string.Empty;

                        var realm = new RealmBase();
                        var configUser = realm.GetByIdConfUsuario((int)App.user.configID);

                        if (configUser.ImageDownload) isImage = "true";
                        else isImage = "false";
                        if (configUser.SoundDownload) isAudio = "true";
                        else isAudio = "false";
                        if (configUser.VideoDownload) isVideo = "true";
                        else isVideo = "false";

                        List<String> ListRetorno = await BookManagement.DownloadBook(AppLeitor.Leitor.isAmostra, item, isImage, isAudio, isVideo);


                        if (!ListRetorno[0].Contains("ERROR"))
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new MainLeitorPage(ListRetorno[1], item));
                        }
                        else
                        {
                            await alertService.ShowAsync("Erro", ListRetorno[1], "Ok");
                        }

                     
                        
                    }
                    else{
                        await alertService.ShowAsync("Atenção", "Exemplar gratuito para Download, não existe uma amostra disponível para exibir.", "Ok");
                    }
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "LivroDetailsViewModel", "OpenToAmostradBookAsync()");
                UserDialogs.Instance.HideLoading();
                await alertService.ShowAsync("Atenção", "Falha ao abrir a amostra. Tente novamente!", "Ok");
            }
            finally
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
                IsBusy = false;
            }
        }

        public async Task OpenToDownloadBookAsync()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    Analytics.TrackEvent("Download do Livro " + Item.Titulo);

                    await DependencyService.Get<ILoadAnimationIntersaberes>().StartAnimationAsync();

                    AppLeitor.Leitor.isAmostra = false;
                    string isImage = string.Empty;
                    string isVideo = string.Empty;
                    string isAudio = string.Empty;

                    var realm = new RealmBase();
                    var configUser = realm.GetByIdConfUsuario((int)App.user.configID);
                   
                    if (configUser.ImageDownload) isImage = "true";
                    else isImage = "false";
                    if (configUser.SoundDownload) isAudio = "true";
                    else isAudio = "false";
                    if (configUser.VideoDownload) isVideo = "true";
                    else isVideo = "false";

                    List<String> ListRetorno = await BookManagement.DownloadBook(AppLeitor.Leitor.isAmostra, item, isImage, isAudio, isVideo);

                    if (!ListRetorno[0].Contains("ERROR"))
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new MainLeitorPage(ListRetorno[1], item));
                    }
                    else{
                        await alertService.ShowAsync("Erro", ListRetorno[1], "Ok");
                    }
                   
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "LivroDetailsViewModel", "OpenToDownloadBookAsync()");
                UserDialogs.Instance.HideLoading();
                await alertService.ShowAsync("Error", "Falha ao abrir o documento. Error: " + exception.Message, "Ok");
            }
            finally
            {
                await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
                this.VerificBoookDownload();
                IsBusy = false;
            }
        }

        public async Task OpenToConfigAsync()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await App.NavigationPush(new ConfigPage());
                IsBusy = false;
            }
        }

        public async Task DeleteAsync()
        {
            try
            {
                bool result = await alertService.ShowAsyncConfirm("Galeria", "Deseja realmente excluir o livro da sua galeria?", "Sim", "Não");
                if (result)
                {
                    //Salva no banco que o livro foi baixado
                    var realm = new RealmBase();
                    var livroBaixado = realm.GetByIdLivroBaixado((int)item.Id);
                    realm.DeleteRealm(livroBaixado);
                     
                    await DependencyService.Get<IConfig>().DirectoryDeleteAll();

                    Analytics.TrackEvent("Deletado o Livro " + Item.Titulo);

                    IsBaixar = false;

                    this.VerificBoookDownload();
                }
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Atenção", "Falha ao deletar livro selecionado.", "OK");
                Function.AppCenterCrashes(exception, "LivroDetailsViewModel", "DeleteAsync()");
            }
        }

        public async Task LerAsync()
        {
            try
            {
                if (!IsBusy)
                {
                    IsBusy = true;

                    await App.NavigationPushPopup(new DownloadBook());

                    string caminho = BookManagement.ReadBook(Convert.ToInt32(Item.Id));

                    await Application.Current.MainPage.Navigation.PushAsync(new MainLeitorPage(caminho, item));
                }
            }
            catch (Exception exception)
            {
                await alertService.ShowAsync("Atenção", "Falha ao abrir o documento. Tente novamente!", "Ok");
                Function.AppCenterCrashes(exception, "LivroDetailsViewModel", "LerAsync()");
            }
            finally
            {
                await App.NavigationPopPopup();
                //await DependencyService.Get<ILoadAnimationIntersaberes>().StopAnimationAsync();
                IsBusy = false;
            }
        }
    }
}