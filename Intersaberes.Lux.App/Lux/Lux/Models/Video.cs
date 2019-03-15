using Xamarin.Forms;
using Lux.ViewsModels;

using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions;

namespace Lux.Models
{
    public class Video : BaseViewModel
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string nome;
        public string Nome
        {
            get { return nome; }
            set { SetProperty(ref nome, value); }
        }

        private bool isPlay;
        public bool IsPlay
        {
            get { return isPlay; }
            set { SetProperty(ref isPlay, value); }
        }

        private string caminho;
        public string Caminho
        {
            get { return caminho; }
            set { SetProperty(ref caminho, value); }
        }

        private int position;
        public int Position
        {
            get { return position; }
            set { SetProperty(ref position, value); }
        }

        private string positionAtual;
        public string PositionAtual
        {
            get { return positionAtual; }
            set { SetProperty(ref positionAtual, value); }
        }

        private int tamanho;
        public int Tamanho
        {
            get { return tamanho; }
            set { SetProperty(ref tamanho, value); }
        }

        private string tamanhoReal;
        public string TamanhoReal
        {
            get { return tamanhoReal; }
            set { SetProperty(ref tamanhoReal, value); }
        }

        public IMediaManager MediaManger => CrossMediaManager.Current;
        public IPlaybackController PlaybackController => CrossMediaManager.Current.PlaybackController;

        public Command PlayerCommand { get; set; }
        public Command PauseCommand { get; set; }
        public Command StopCommand { get; set; }

        //this.volumeLabel.Text = "Volume (0-" + CrossMediaManager.Current.VolumeManager.MaxVolume + ")";
        ////Initialize Volume settings to match interface
        //int.TryParse(this.volumeEntry.Text, out var vol);
        //CrossMediaManager.Current.VolumeManager.CurrentVolume = vol;
        //CrossMediaManager.Current.VolumeManager.Mute = false;

        //CrossMediaManager.Current.PlayingChanged += (sender, e) =>
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //    {
        //        ProgressBar.Progress = e.Progress;
        //        Duration.Text = "" + e.Duration.TotalSeconds + " seconds";
        //    });
        //};
    }
}
