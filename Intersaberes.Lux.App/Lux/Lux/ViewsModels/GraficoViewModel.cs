using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Lux.ViewsModels
{

    public class GraficoViewModel : BaseViewModel
    {
        public ObservableCollection<string> ListGraficos{ get; set; }

        public Command CloseCommand { get; set; }

        public GraficoViewModel(List<string> listGraficos)
        {

            if (Device.RuntimePlatform == Device.Android){

                ListGraficos = new ObservableCollection<string>();

                foreach (string endereco in listGraficos){
                    var newEndereco  = "file://" + endereco;
                    ListGraficos.Add(newEndereco);
                }

            }
            else { // iOS
                ListGraficos = new ObservableCollection<string>(listGraficos);
            }

            CloseCommand = new Command(async () => await Close());
        }

        async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
