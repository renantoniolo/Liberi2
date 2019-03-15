using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Lux.Views.Animation
{
    public partial class LoadAnimationIntersaberes : PopupPage
    {
        private bool verf = true;

        public LoadAnimationIntersaberes()
        {
            InitializeComponent();
            // imagem default
            image.Source = "loader_liberi";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            while (verf)
            {
                await Task.Delay(3);
                image.Opacity = 0;
                await image.FadeTo(1,1500);
                await Task.Delay(10);

            }
        }

        public void SetImageName(string nameImg)
        {
            if (!String.IsNullOrEmpty(nameImg))
                image.Source = nameImg;
        }

        public void CloseLoadAnimation()
        {
            try
            {
                verf = false;
                base.OnBackButtonPressed();
                base.OnBackgroundClicked();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro: " + ex);
            }
        }

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

    }
}
