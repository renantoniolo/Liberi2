using System;

namespace Lux.Interfaces
{
    public interface IBrightnessService
    {
        void SetBrightness(float brightness);

        float GetBrightness();
    }
}
