using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class ImagemLeitorPage : ContentPage
    {
        public ImagemLeitorPage(List<string> listImagens)
        {
            InitializeComponent();

            BindingContext = new ImagenViewModel(listImagens);
        }
    }
}
