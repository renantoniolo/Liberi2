using System.Collections.Generic;
using Lux.ViewsModels;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class GraficoLeitorPage : ContentPage
    {
        public GraficoLeitorPage(List<string> listGraficos)
        {
            InitializeComponent();

            BindingContext = new GraficoViewModel(listGraficos);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
