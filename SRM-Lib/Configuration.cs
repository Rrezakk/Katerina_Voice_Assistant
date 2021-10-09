using System;
using System.Collections.Generic;
using System.Text;

namespace k3na_voice
{
    public static class Configuration
    {
        public const int SampleRate = 48000;
        public const int BitResolution = 16;
        public const int Channels = 1;
        public const int WaveInputBuffer = 100;
        public const int SoundDevice = 0; //3 - winaudio
        public const int Treshold = 1400;

        public const int PreRecordBytes = 4800;
        public const float BasicEmaAlpha = 0.9f;//α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных для хранилища сэмплов
        public const float DefaultEmaAlpha = 0.3f;//α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных:
        public const float NoiseMeterAlpha = 0.08f;
    }
}
