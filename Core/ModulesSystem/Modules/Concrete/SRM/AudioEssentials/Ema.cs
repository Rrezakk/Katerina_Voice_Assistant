using System;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
    public class Ema
    {
        private readonly object _emaLocker = new();
        private int _previous = 1;
        public float
            Alpha = Configuration
                .DefaultEmaAlpha; /*α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных:
                    чем выше его значение, тем больший удельный вес имеют новые наблюдения случайной величины, и тем меньший старые;*/
        public int Iterate(int value)
        {
            lock (_emaLocker)
            {
                _previous = (int) (Alpha * Math.Abs(value) + (1 - Alpha) * _previous);
                return _previous;
            }
        }
        public int GetValue()
        {
            lock (_emaLocker)
            {
                return _previous;
            }
        }
    }
}