using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Lux.Models;
using Lux.Helpers;
using Xamarin.Forms;
using Plugin.MediaManager;
using System.Diagnostics;

namespace Lux.ViewsModels
{
    public class VideoViewModel : BaseViewModel
    {
        public ObservableCollection<Video> ListVideos { get; set; }

        public Command CloseCommand { get; set; }

        public VideoViewModel(List<string> listVideos)
        {
            int count = 0;
            this.ListVideos = new ObservableCollection<Video>();

            CloseCommand = new Command(async () => await Close());
            try
            {
                foreach (var video in listVideos)
                {
                    ListVideos.Add(new Video()
                    {
                        Id = ListVideos.Count,
                        Nome = video.Split('/').Last(),
                        Caminho = string.Format("file://{0}",video),
                        Position = 0,
                        IsPlay = false,

                        PlayerCommand = new Command((object obj) => Play(obj)),
                        StopCommand = new Command((object obj) => Stop(obj)),
                        PauseCommand = new Command((object obj) => Pause(obj))
                    });

                    ListVideos[count].Tamanho = Convert.ToInt32(Math.Ceiling(ListVideos[count].MediaManger.Duration.TotalSeconds));
                    ListVideos[count].Tamanho = 56;
                    ListVideos[count].TamanhoReal = (ListVideos[count].MediaManger.Duration.TotalSeconds).ToString();
                    ListVideos[count].PositionAtual = string.Format("{0:0.00}", ListVideos[count].TamanhoReal);

                    count++;
                }
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "VideoViewModel", "Construtor()");
            }
        }

        internal void ThisDisAppearing()
        {
            try
            {
                this.Stop(0);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        internal void ThisOnAppearing()
        {
            try
            {
                this.Play(0);
            }
            catch(Exception ex){

                Debug.WriteLine(ex.Message);
            }
        }

        //PlayBackController
        void Play(object obj)
        {
            try
            {
                int id = Convert.ToInt32(obj);
                ListVideos[id].PlaybackController.Play();
                //CrossMediaManager.Current.Play(ListVideos[id].Caminho,Plugin.MediaManager.Abstractions.Enums.MediaFileType.Video);
                ListVideos[id].IsPlay = true;

                ListVideos[id].TamanhoReal = (ListVideos[id].MediaManger.Duration.TotalSeconds).ToString();
                ListVideos[id].MediaManger.PlayingChanged += (sender, e) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ListVideos[id].Position = (int)Math.Ceiling(e.Progress);
                        ListVideos[id].PositionAtual = string.Format("{0:mm\\:ss}", ListVideos[id].MediaManger.Position);
                    });
                };
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "VideoViewModel", "Play()");
            }
        }
        void Pause(object obj)
        {
            try
            {
                int id = Convert.ToInt32(obj);
                ListVideos[id].PlaybackController.Pause();
                ListVideos[id].IsPlay = false;
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "VideoViewModel", "Pause()");
            }
        }
        void Stop(object obj)
        {
            try
            {
                int id = Convert.ToInt32(obj);
                ListVideos[id].PlaybackController.Stop();
                ListVideos[id].IsPlay = false;
            }
            catch (Exception ex)
            {
                Function.AppCenterCrashes(ex, "VideoViewModel", "Stop()");
            }
        }

        async Task Close()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}
