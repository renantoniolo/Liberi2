using System;
using System.Threading.Tasks;

namespace Lux.Interfaces
{
    public interface ILoadAnimationIntersaberes
    {
        Task StartAnimationAsync();

        Task StopAnimationAsync();

        void SetImageName(string nameImg);

    }
}
