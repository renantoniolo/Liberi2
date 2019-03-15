using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Lux.Views;
using Lux.Models;
using Lux.Services;
using Lux.Interfaces;

using Xamarin.Forms;
using PCLAppConfig;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using DLToolkit.Forms.Controls;
using Rg.Plugins.Popup.Services;
using MonkeyCache.FileStore;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.AppCenter.Push;
using Lux.Helpers;
using Lux.Infra;
using Realms;
using Acr.UserDialogs;
using Lux.ViewsModels;

namespace Lux
{
    public partial class App : Application
    {
        public static bool isLeitor = false;
        public static string token = string.Empty;
        public static Usuario user = null;
        //Variavel que armazena o caminho do livro que o usuario abriu e ainda não foram deletados 
        public static List<string> listLivroDir = new List<string>();
        public static MasterDetailPage MasterDetail { get; set; }
        public static EPUBViewModel ePUBViewModel { get; set; }

        public App()
        {
            InitializeComponent();
          
            //RegistrandoDependencias
            DependencyService.Register<IAlertService, AlertService>();
            DependencyService.Register<ILoadAnimationIntersaberes, LoadAnimationService>();
            FlowListView.Init();

            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            ConfigurationManager.Initialise(assembly.GetManifestResourceStream("Lux.App.config"));

            Barrel.ApplicationId = "app_lux_barrel";

            try
            {
                if (!App.Current.Properties.ContainsKey("CreateNewBD"))
                {
                    try
                    {
                        // faz o login novamente
                        MainPage = new NavigationPage(new LoginPage());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("'Erro ao criar a nova base de dados: " + ex.Message);
                    }

                }
                else // esta com a base de dados correta!
                {

                    if (App.Current.Properties.ContainsKey("UserCPF") &&
                    !String.IsNullOrEmpty(Application.Current.Properties["UserCPF"] as string))
                    {
                        DateTime dataLogon = Convert.ToDateTime(App.Current.Properties["DateIsLogado"].ToString());

                        if (dataLogon < DateTime.Now.AddDays(7))
                        {
                            string userCPF = App.Current.Properties["UserCPF"].ToString();

                            var realm = new RealmBase();

                            App.user = realm.GetByCpf(userCPF);

                            MainPage = new NavigationPage(new MainMasterDetailPage());

                        } // pede o login novamanete
                        else
                        {
                            App.Current.Properties["UserCPF"] = "";
                            App.Current.Properties["DateIsLogado"] = "";
                            App.Current.SavePropertiesAsync();

                            MainPage = new NavigationPage(new LoginPage());
                        }
                    }
                    else MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        public async static Task NavigationMaster(Page page)
        {
            MasterDetail.IsPresented = false; //Fechar a aba da master 

            MasterDetail.Detail = new NavigationPage(page);
        }


        public  static void NavigationMainPage(Page page)
        {
            try
            {
                //App.Current.MainPage =  page;
                Application.Current.MainPage = new NavigationPage(page);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar a MainPage. " + ex.Message);
            }
        }

        public async static Task NavigationPush(Page page)
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar a Navigation Page. " + ex.Message);
            }
        }
        public async static Task NavigationPop()
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao fechar o PopModal. " + ex.Message);
            }
        }
        public async static Task NavigationPushModal(Page page)
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PushModalAsync(page);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar a Modal. " + ex.Message);
            }
        }
        public async static Task NavigationPopModal()
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PopModalAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao fechar o PopModal. " + ex.Message);
            }
        }
        public async static Task NavigationPushPopup(PopupPage popup)
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PushPopupAsync(popup);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao carregar o Popup. " + ex.Message);
            }
        }
        public async static Task NavigationPopPopup()
        {
            try
            {
                await App.MasterDetail.Detail.Navigation.PopPopupAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao fechar o Popup. " + ex.Message);
            }
        }
        public async static Task NavigateToClosePopup()
        {
            try
            {
                var stack = PopupNavigation.PopupStack;
                if (stack.Count > 0) await PopupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Falha ao fechar o Popup. " + ex.Message);
                return;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app start
            isLeitor = false;

            AppCenter.Start("ios=a25d705f-e8a0-4913-b8c0-7e374edefb6a;" + "android=4d26fa6b-7182-4c24-9a81-79f0face2afe;", typeof(Analytics), typeof(Crashes));
			AppCenter.Start("ios=a25d705f-e8a0-4913-b8c0-7e374edefb6a;" + "android=4d26fa6b-7182-4c24-9a81-79f0face2afe;", typeof(Distribute));
            AppCenter.Start("ios=a25d705f-e8a0-4913-b8c0-7e374edefb6a;" + "android=4d26fa6b-7182-4c24-9a81-79f0face2afe;", typeof(Push));

            //Crashes.GenerateTestCrash();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps

            // ao sair sempre exclui as pastas
            DependencyService.Get<IConfig>().DirectoryDeleteAll();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes

            // estamos com o leitor aberto?
            if (App.isLeitor)
            {
                var idLivro = "";

                if (Application.Current.Properties.ContainsKey("BookIdRead"))
                {
                    idLivro = Application.Current.Properties["BookIdRead"].ToString();
                }

                // recarrega o livro para eu possa a ler ele novamente 
                //BookManagement.ReadBook(Convert.ToInt32(idLivro));

                // Retorna em segundo plano os arquivos do livro
                Task.Run(() => {
                    string endereco = BookManagement.ReadBook(Convert.ToInt32(idLivro));
                    if (!String.IsNullOrEmpty(endereco)) Debug.WriteLine("Livro descriptografado no endereço: " + endereco);
                });

                Debug.WriteLine("O livro que estava sendo lido é " + idLivro);
            }

        }

    }
}