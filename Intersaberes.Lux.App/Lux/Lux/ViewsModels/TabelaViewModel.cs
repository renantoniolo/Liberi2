using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Lux.ViewsModels
{

    public class TabelaViewModel : BaseViewModel
    {
        public ObservableCollection<string> ListTabelas { get; set; }

        public Command CloseCommand { get; set; }

        public TabelaViewModel(List<string> listTabelas)
        {
            if (Device.RuntimePlatform == Device.Android)
            { // Android

                ListTabelas = new ObservableCollection<string>();

                foreach (string endereco in listTabelas)
                {
                    var newEndereco = "file://" + endereco;
                    ListTabelas.Add(newEndereco);
                }
            }
            else // iOS
            {
                ListTabelas = new ObservableCollection<string>(listTabelas);
            }

            CloseCommand = new Command(async () => await Close());
        }

        async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
