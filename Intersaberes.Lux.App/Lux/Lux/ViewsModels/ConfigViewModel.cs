using System;
using System.Threading.Tasks;

using Lux.Interfaces;
using Lux.Models;

using Xamarin.Forms;
using Lux.Helpers;
using Lux.Infra;

namespace Lux.ViewsModels
{
    public class ConfigViewModel : BaseViewModel
    {
        public ConfigUsuario config { get; set; }
        public Command ExecuteCommand { get; set; }

        public ConfigViewModel()
        {
            var realm = new RealmBase();
            config = realm.GetByIdConfUsuario((int)App.user.configID);

            ExecuteCommand = new Command(async() => await SalveConfig());
        }

        async Task SalveConfig()
        {
            var alert = DependencyService.Get<IAlertService>();

            try
            {
                var realm = new RealmBase();
                realm.InsertUpdate(config);

                await alert.ShowAsync("Sucesso", "Consigurações salva com sucesso","OK");
            }
            catch (Exception exception)
            {
                await alert.ShowAsync("Erro", "Falha ao salvar as configurações.", "OK");
                Function.AppCenterCrashes(exception, "ConfigViewModel", "SaveConfig()");
            }
        }
    }
}
