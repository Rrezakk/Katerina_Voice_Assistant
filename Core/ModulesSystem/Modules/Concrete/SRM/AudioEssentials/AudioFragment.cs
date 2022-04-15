using System;
using System.Collections.Generic;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
    public class AudioFragment : ICloneable
    {
        private byte[] _audioBytes = new byte[48000];
        private short[] _audioShorts = new short[24000];
        private int _audioShortsPtr =0;
        private int _ptr =0;
        public int MaxVolume =0;
        public int IntegralVolume =0;
        public float Amplitude = 0f;
        public float Db = 0f;
        public int AudioConsistence = 0;

        public void AppendBytes(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i % 2 == 0) _audioShorts[_audioShortsPtr++] = BitConverter.ToInt16(bytes, i);

                _audioBytes[_ptr++] = bytes[i];
            }
        }

        public bool IsFull()
        {
            return _ptr == 48000;
        }

        public void Process()
        {
            var ampAccumulator = 0f;
            foreach (int amp in _audioShorts)
            {
                try
                {
                    var absAmp = Math.Abs(amp);
                    if (MaxVolume < absAmp) MaxVolume = absAmp; //Максимальная громкость звука
                    IntegralVolume += absAmp;

                    ampAccumulator += absAmp;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Process in AudioEssentials error: {e}");
                }
            }

            Amplitude = ampAccumulator / (_audioShorts.Length * 32767);
            Db = 20 * MathF.Log10(Amplitude);

            //maybe will be useful: https://ru.stackoverflow.com/questions/818576/%D0%9F%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D1%87%D0%B0%D1%81%D1%82%D0%BE%D1%82%D1%8B-%D0%B7%D0%B2%D1%83%D0%BA%D0%B0-%D1%81-%D0%BC%D0%B8%D0%BA%D1%80%D0%BE%D1%84%D0%BE%D0%BD%D0%B0
            //also: https://dropsofai.com/sound-wave-basics-every-data-scientist-must-know-before-starting-analysis-on-audio-data/
            //also: https://towardsdatascience.com/understanding-audio-data-fourier-transform-fft-spectrogram-and-speech-recognition-a4072d228520
            //and maybe: https://www.cyberforum.ru/windows-forms/thread1706717.html
                
            AudioConsistence = (int)(0.7f * ((float)IntegralVolume / _audioShorts.Length) + 0.3f * (MaxVolume));
                
            if (AudioConsistence < Configuration.Treshold)//Experimental
            {
                NoiseMeter.TakeMeasure(AudioConsistence);
            }
            Console.WriteLine($"{Amplitude} - {Db}dB");
        }
        public IEnumerable<byte> GetBytes()=>_audioBytes;
        public IEnumerable<short> GetShorts()=>_audioShorts;
        public object Clone()=>new AudioFragment {
            _audioBytes = _audioBytes,
            MaxVolume = MaxVolume,
            _ptr = _ptr,
            IntegralVolume = IntegralVolume,
            _audioShorts = _audioShorts,
            _audioShortsPtr = _audioShortsPtr
        };
    }
}