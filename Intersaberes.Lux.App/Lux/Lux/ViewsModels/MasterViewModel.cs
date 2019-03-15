using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Lux.Views;
using Lux.Helpers;
using Lux.Views.Popup;
using Xamarin.Forms;

namespace Lux.ViewsModels
{
    public class MasterViewModel : BaseViewModel
    {
        public ICommand MenuCommandCommand { get; set; }

        public MasterViewModel()
        {
            Title = "Menu";

            BoolIconLabel = true;
            BoolImageSource = false;

            MenuCommandCommand = new Command(async (object x) => await ExecuteMenuCommandCommand(x), (x) => !IsBusy);
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

        internal void ThisOnAppearing()
        {
            LoadPerfil();
        }

        public void LoadPerfil()
        {
            try
            {
                if (App.user != null)
                {
                    InputNome = App.user.Nome;
                    if (!String.IsNullOrEmpty(App.user.Foto))
                    {
                        ImagemConverter imagem = new ImagemConverter();
                        ImageSource = imagem.GetImageBase64ToImageSource(App.user.Foto);
                        BoolIconLabel = false;
                        BoolImageSource = true;
                    }
                }

            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MasterViewModel", "LoadPerfil()");
            }
        }

        async Task ExecuteMenuCommandCommand(object x)
        {
            int item = Convert.ToInt32(x);

            try
            {

                switch (item)
                {
                    case 0:
                        await App.NavigationMaster(new HomePage());
                        break;
                    case 1:
                        await App.NavigationMaster(new BibliotecaPage());
                        break;
                    case 2:
                        // loja intersaberes
                        break;
                    case 3:
                        await App.NavigationMaster(new MeusLivrosPage());
                        break;
                    case 4:
                        //minhas compras
                        break;
                    case 5:
                        await App.NavigationMaster(new PerfilPage(this));
                        break;
                    case 6:
                        await App.NavigationMaster(new ConfigPage());
                        break;
                    case 7:
                        await LogoutUser();
                        App.NavigationMainPage(new LoginPage());

                        break;
                    case 8:
                        await App.NavigationPushPopup(new SobreAplicativoPop());
                        break;
                    case 9:
                        await App.NavigationPushModal(new SearchLivroGlobalPage());
                        App.MasterDetail.IsPresented = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MasterViewModel", "ExecuteMenuCommandCommand()");
            }
        }

        private async Task LogoutUser()
        {
            try
            {
                App.Current.Properties["UserCPF"] = "";
                App.Current.Properties["DateIsLogado"] = "";
                await App.Current.SavePropertiesAsync();
            }
            catch (Exception exception)
            {
                Function.AppCenterCrashes(exception, "MasterViewModel", "LogoutUser()");
            }
        }
    }
}
