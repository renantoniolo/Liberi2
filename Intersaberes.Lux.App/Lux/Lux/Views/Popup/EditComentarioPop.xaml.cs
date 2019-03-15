using System;
using Lux.Models;
using Lux.ViewsModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Syncfusion.SfRating.XForms;
using Xamarin.Forms;

namespace Lux.Views.Popup
{
    public partial class EditComentarioPop : PopupPage
    {
        EditComentarioViewModel viewModel;
        ComentarioViewModel comentarioViewModel;

        public EditComentarioPop(Comentario comment, ComentarioViewModel comentarioViewModel)
        {
            InitializeComponent();
            this.comentarioViewModel = comentarioViewModel;
            BindingContext = viewModel = new EditComentarioViewModel(comment,comentarioViewModel);
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

        public async void ClosePopup(Object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        public void OnRatingValue(object sender, Syncfusion.SfRating.XForms.ValueEventArgs e)
        {
            RatingSettings();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RatingSettings();
        }

        void RatingSettings()
        {
            SfRatingSettings ratingSettings = new SfRatingSettings();
            if (viewModel.Aval < 2)
            {
                ratingSettings.RatedFill = Color.Red;
                ratingSettings.RatedStroke = Color.Red;
            }
            else if (viewModel.Aval <= 3)
            {
                ratingSettings.RatedFill = Color.Yellow;
                ratingSettings.RatedStroke = Color.Yellow;
            }
            else
            {
                ratingSettings.RatedFill = Color.Green;
                ratingSettings.RatedStroke = Color.Green;
            }

            rating.RatingSettings = ratingSettings;
        }
    }
}
