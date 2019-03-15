using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Lux.Models;
using Lux.Helpers;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;

namespace Lux.ViewsModels
{
    public class AudioViewModel : BaseViewModel
    {
        public ObservableCollection<Audio> ListAudios { get; set; }
        public Command CloseCommand { get; set; }
        public Command LeftCommand { get; set; }
        public Command RightCommand { get; set; }
        public Command StopCommand { get; set; }

        private bool isLeft;
        public bool IsLeft
        {
            get { return isLeft; }
            set { SetProperty(ref isLeft, value); }
        }

        private bool isRight;
        public bool IsRight
        {
            get { return isRight; }
            set { SetProperty(ref isRight, value); }
        }

        private int carouselPosition = 0;
        public int CarouselPosition
        {
            get { return carouselPosition; }
            set { SetProperty(ref carouselPosition, value); }
        }

        public AudioViewModel(List<string> listAudios)
        {
            int count = 0;
            this.ListAudios = new ObservableCollection<Audio>();

            try
            {
                IsLeft = false;
                if (ListAudios.Count > 0) IsRight = true;
                else IsRight = false;

                foreach (var audio in listAudios)
                {
                    ListAudios.Add(new Audio()
                    {
                        Id = ListAudios.Count(),
                        Nome = audio.Split('/').Last(),
                        Caminho = audio,
                        Position = 0,
                        IsPlay = false,
                        Stream = new FileStream(audio, FileMode.Open),

                        PlayerCommand = new Command((object obj) => PlayerAction(obj)),
                        StopCommand = new Command((object obj) => Stop(obj))
                    });

                    ListAudios[count].Player.Load(ListAudios[count].Stream);
                    ListAudios[count].Tamanho = Convert.ToInt32(Math.Ceiling(ListAudios[count].Player.Duration));
                    ListAudios[count].TamanhoReal = (ListAudios[count].Player.Duration / 60).ToString().Remove(4).Replace(".", ":").Replace(",", ":");
                    ListAudios[count].PositionAtual = string.Format("{0}/{1}", "0:00", ListAudios[count].TamanhoReal);
                    count++;
                }
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "Construtor()");
            }
            finally
            {
                CloseCommand = new Command(async () => await Close());
                LeftCommand = new Command(SetLeft);
                RightCommand = new Command(SetRight);
            }
        }

        async Task Close()
        {
            try
            {
                for (int i = 0; i < ListAudios.Count; i++)
                {
                    if (ListAudios[i].Player.IsPlaying) ListAudios[i].Player.Stop();
                    ListAudios[i].Stream.Close();
                }
                await Application.Current.MainPage.Navigation.PopPopupAsync(true);
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "Close()");
            }
        }

        void PlayerAction(object obj)
        {
            try
            {
                int id = Convert.ToInt32(obj);
                if (!ListAudios[id].Player.IsPlaying) Play(id);
                else Pause(id);
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "PlayerAction()");
            }
        }

        void Play(int id)
        {
            try
            {
                StopAllPlayer(id);

                ListAudios[id].Player.Play();
                ListAudios[id].IsPlay = true;

                new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    while (ListAudios[id].Player.IsPlaying)
                    {
                        ListAudios[id].Position = Convert.ToInt32(Math.Ceiling(ListAudios[id].Player.CurrentPosition));

                        string positionString = (ListAudios[id].Player.CurrentPosition / 60).ToString().Replace(".", ":").Replace(",", ":");
                        if (positionString.Length < 5)
                            ListAudios[id].PositionAtual = string.Format("{0}/{1}", positionString, ListAudios[id].TamanhoReal);
                        else
                            ListAudios[id].PositionAtual = string.Format("{0}/{1}", positionString.Remove(4), ListAudios[id].TamanhoReal);
                    }
                })).Start();
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "Play()");
            }
        }

        void Pause(int id)
        {
            try
            {
                ListAudios[id].Player.Pause();
                ListAudios[id].IsPlay = false;
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "Pause()");
            }
        }

        void Stop(object obj)
        {
            try
            {
                int id = Convert.ToInt32(obj);
                ListAudios[id].Player.Stop();
                ListAudios[id].IsPlay = false;

                ListAudios[id].Position = 0;
                ListAudios[id].PositionAtual = string.Format("{0}/{1}", "0:00", ListAudios[id].TamanhoReal);
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "Stop()");
            }
        }
        void StopAllPlayer(int id)
        {
            try
            {
                for (int i = 0; i < ListAudios.Count; i++)
                {
                    if (ListAudios[i].Player.IsPlaying && i != id)
                    {
                        ListAudios[i].Player.Stop();

                        ListAudios[i].Position = 0;
                        ListAudios[i].PositionAtual = string.Format("{0}/{1}", "0:00", ListAudios[i].TamanhoReal);
                        ListAudios[i].IsPlay = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "AudioViewModel", "StopAllPlayer()");
            }
        }

        public void SetPositionPlay(int id,double position)
        {
            ListAudios[id].Player.Seek(position);
            PlayerAction(id);
        }

        public void SetArrow(int position)
        {
            CarouselPosition = position;
            IsRight = true;
            IsLeft = true;

            if (carouselPosition == 0) IsLeft = false;
            else if (carouselPosition == ListAudios.Count - 1) IsRight = false;
        }

        void SetLeft() =>  CarouselPosition--;
        void SetRight() => CarouselPosition++;
    }
}
