using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Lux.Interfaces;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace Lux.Services
{
    public class PermissionService
    {

        private IAlertService _alertService;

        public PermissionService()
        {
            this._alertService = DependencyService.Get<IAlertService>();

        }

        public async Task startPermissionService()
        {

            PermissionStatus permission = PermissionStatus.Denied;

            try
            {
                permission = await PermissionStorage();
                if (permission != PermissionStatus.Granted) await startPermissionService();

                if (Device.RuntimePlatform == Device.Android)
                {
                    permission = await PermissionCamera();
                    if (permission != PermissionStatus.Granted) await startPermissionService();
                }

            }
            catch(Exception ex){
                Debug.WriteLine(ex.Message);
            }

        }

        private async Task<PermissionStatus> PermissionPhone()
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Phone);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Phone))
                {
                    await _alertService.ShowAsync("Preciso de informações", "Vou precisar acessar as configurações do seu telefone.", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Phone });
                status = results[Permission.Phone];
            }

            return status;

        }

        private async Task<PermissionStatus> PermissionLocation()
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await this._alertService.ShowAsync("Preciso de localização", "Vou precisar usar a sua localização.", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                status = results[Permission.Location];
            }

            return status;

        }

        private async Task<PermissionStatus> PermissionCamera()
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                {
                    await _alertService.ShowAsync("Preciso da Camera", "Vou precisar usar a sua camera", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                status = results[Permission.Camera];
            }

            return status;

        }

        private async Task<PermissionStatus> PermissionStorage()
        {

            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                {
                    await _alertService.ShowAsync("Preciso do Cartão de memória", "Vou precisar usar o cartão de memória", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                status = results[Permission.Storage];
            }

            return status;

        }

    }
}
