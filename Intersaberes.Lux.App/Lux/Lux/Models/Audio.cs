using System.IO;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer;
using Lux.ViewsModels;

namespace Lux.Models
{
    public class Audio : BaseViewModel
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

        public Stream Stream { get; set; }
        public ISimpleAudioPlayer Player { get; set; } = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

        public Command PlayerCommand { get; set; }
        public Command StopCommand { get; set; }
    }
}
