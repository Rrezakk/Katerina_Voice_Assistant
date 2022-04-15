using System;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
    public static class NoiseMeter//Experimental
    {
        private static readonly Ema Ema = new();
        static NoiseMeter()
        {
            Ema.Alpha = Configuration.NoiseMeterAlpha;
        }
        public static int TakeMeasure(int volume)
        {
            var vol = Ema.Iterate(volume);
#if dia
            Console.WriteLine($"Noise-meter: {vol}");
#endif
            return vol;
        }
        public static int GetVolume() => Ema.GetValue();
    }
}