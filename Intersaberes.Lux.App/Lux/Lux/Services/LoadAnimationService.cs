using System;
using System.Threading.Tasks;
using Lux.Interfaces;
using Lux.Views.Animation;

namespace Lux.Services
{
    public class LoadAnimationService : ILoadAnimationIntersaberes
    {
        private LoadAnimationIntersaberes loadAnimationIntersaberes;

        public LoadAnimationService()
        {
            loadAnimationIntersaberes = new LoadAnimationIntersaberes();
        }

        public void SetImageName(string nameImg)
        {
            loadAnimationIntersaberes.SetImageName(nameImg);
        }

        public async Task StartAnimationAsync()
        {
            await App.NavigationPushPopup(loadAnimationIntersaberes);
        }

        public async Task StopAnimationAsync()
        {
            try
            {
                loadAnimationIntersaberes.CloseLoadAnimation();
                await App.NavigateToClosePopup();
            }
            catch{}
        }
    }
}
