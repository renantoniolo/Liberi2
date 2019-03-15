using System;
using System.Collections.Generic;
using Lux.ViewsModels;
using Xamarin.Forms;

namespace Lux.Views
{
    public partial class VideoLeitorPage : ContentPage
    {
        public VideoLeitorPage(List<string> listVideos)
        {
            InitializeComponent();

            BindingContext = new VideoViewModel(listVideos);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ((VideoViewModel)BindingContext).ThisOnAppearing();
        }

        protected override void OnDisappearing()
        {
            ((VideoViewModel)BindingContext).ThisDisAppearing();

        }
    }
}
