using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using System.IO;

namespace Lux.ViewsModels
{
    public class ImagenViewModel : BaseViewModel
    {
        public ObservableCollection<string> ListImagens { get; set; }
        public Command CloseCommand { get; set; }

        public ImagenViewModel(List<string> listImagens)
        {
            bool a = File.Exists(listImagens[0]);

            ListImagens = new ObservableCollection<string>(listImagens);
            CloseCommand = new Command(async () => await Close());
        }

        async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
