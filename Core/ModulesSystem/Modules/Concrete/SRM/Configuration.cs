using System.Globalization;
using Superpower;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM
{
    public static class Configuration
    {
        public static int SampleRate => int.Parse(Core.AppConfiguration.GetVariable("SampleRate"));

        public static int BitResolution => int.Parse(Core.AppConfiguration.GetVariable("BitResolution"));
        public static int Channels => int.Parse(Core.AppConfiguration.GetVariable("Channels"));
        public static int WaveInputBuffer => int.Parse(Core.AppConfiguration.GetVariable("WaveInputBuffer"));
        public static int SoundDevice => int.Parse(Core.AppConfiguration.GetVariable("SoundDevice")); //3 - winaudio
        public static int Treshold => int.Parse(Core.AppConfiguration.GetVariable("Treshold"));

        public static int PreRecordBytes => int.Parse(Core.AppConfiguration.GetVariable("PreRecordBytes"));
        public static float DefaultEmaAlpha => float.Parse(Core.AppConfiguration.GetVariable("DefaultEmaAlpha"), CultureInfo.InvariantCulture);//α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных по умолчанию
        public static float AudistEmaAlpha => float.Parse(Core.AppConfiguration.GetVariable("AudistEmaAlpha"),CultureInfo.InvariantCulture);//α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных для хранилища сэмплов
        public static float NoiseMeterAlpha => float.Parse(Core.AppConfiguration.GetVariable("NoiseMeterAlpha"), CultureInfo.InvariantCulture);//α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных:
    }
}
