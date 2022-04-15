#define dia //for diagnostics
//#define deb //other diagnostics
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NAudio.Wave;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM
{
    public static class AudioEssentials
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
        public class AudioFragment : ICloneable
        {
            private byte[] _audioBytes = new byte[48000];
            private short[] _audioShorts = new short[24000];
            private int _audioShortsPtr =0;
            private int _ptr =0;
            public int MaxVolume =0;
            public int IntegralVolume =0;
            public float Amplitude = 0f;
            public float dB = 0f;
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
                dB = 20 * MathF.Log10(Amplitude);

                //maybe will be useful: https://ru.stackoverflow.com/questions/818576/%D0%9F%D0%BE%D0%BB%D1%83%D1%87%D0%B5%D0%BD%D0%B8%D0%B5-%D1%87%D0%B0%D1%81%D1%82%D0%BE%D1%82%D1%8B-%D0%B7%D0%B2%D1%83%D0%BA%D0%B0-%D1%81-%D0%BC%D0%B8%D0%BA%D1%80%D0%BE%D1%84%D0%BE%D0%BD%D0%B0
                //also: https://dropsofai.com/sound-wave-basics-every-data-scientist-must-know-before-starting-analysis-on-audio-data/
                //also: https://towardsdatascience.com/understanding-audio-data-fourier-transform-fft-spectrogram-and-speech-recognition-a4072d228520
                //and maybe: https://www.cyberforum.ru/windows-forms/thread1706717.html
                
                AudioConsistence = (int)(0.7f * ((float)IntegralVolume / _audioShorts.Length) + 0.3f * (MaxVolume));
                
                if (AudioConsistence < Configuration.Treshold)//Experimental
                {
                    NoiseMeter.TakeMeasure(AudioConsistence);
                }
                Console.WriteLine($"{Amplitude} - {dB}dB");
                //Console.WriteLine($"Amp: {TotalVolume/_audioShorts.Length} ma: {MaxVolume} -> {AudioConsistence}");
            }

            public IEnumerable<byte> GetBytes()
            {
                return _audioBytes;
            }

            public IEnumerable<short> GetShorts()
            {
                return _audioShorts;
            }

            public object Clone()
            {
                return new AudioFragment
                {
                    _audioBytes = _audioBytes,
                    MaxVolume = MaxVolume,
                    _ptr = _ptr,
                    IntegralVolume = IntegralVolume,
                    _audioShorts = _audioShorts,
                    _audioShortsPtr = _audioShortsPtr
                };
            }
        }

        public class AudioStorage
        {
            private readonly object _fragmentsLocker = new object();
            private List<AudioFragment> _fragments = new List<AudioFragment>();
            public readonly Ema AmplitudeEma = new Ema();
            public void Add(AudioFragment fragment)
            {
                lock (_fragmentsLocker)
                {
                    _fragments.Add((AudioFragment)fragment.Clone());
                }
            }
            public List<AudioFragment> GetFragments()
            {
                lock (_fragmentsLocker)
                {
                    return _fragments;
                }
            }
            public int GetFragmentsCount()
            {
                lock (_fragmentsLocker)
                {
                    return _fragments.Count;
                }
            }
            public byte[] GetSolid()
            {
                lock (_fragmentsLocker)
                {
                    var solid = new byte[_fragments.Count * 48000];
                    var ptr = 0;
                    foreach (var t in _fragments
                                 .Select(audioFragment => audioFragment.GetBytes())
                                 .SelectMany(fragmentBytes => fragmentBytes))
                    {
                        solid[ptr++] = t;
                    }

                    return solid;
                }
            }

            public short[] GetSolidWr(int id)
            {
                lock (_fragmentsLocker)
                {
                    var solid = new short[_fragments.Count * 24000];
                    var ptr = 0;
                    id = id <= _fragments.Count ? id : _fragments.Count;
                    for (var i = 0; i < id; i++)
                    {
                        var audioFragment = _fragments[i];
                        var fragmentsShorts = audioFragment.GetShorts();
                        foreach (var t in fragmentsShorts) solid[ptr++] = t;
                    }
                    var tmp = new List<AudioFragment>(); //Чтобы не удалить лишнего
                    for (var i = id + 1; i < _fragments.Count; i++) tmp.Add(_fragments[i]);
                    _fragments = new List<AudioFragment>();
                    foreach (var tFragment in tmp) _fragments.Add(tFragment);
                    return solid;
                }
            }

            public int Count()
            {
                lock (_fragmentsLocker)
                {
                    return _fragments.Count;
                }
            }

            public void Clear()
            {
                lock (_fragmentsLocker)
                {
                    _fragments = new List<AudioFragment>();
                }
            }
        }
        public static class AudioFileProcessor
        {
            private static int _pathCtr;

            private static string GenerateNewPath()
            {
                return $"{_pathCtr++}.wav";
            }

            public static string SaveWav(short[] audio)
            {
                var binaryBuffer = ToBinary(audio, 0, audio.Length);
                //Console.WriteLine($"Audio saving: {audio.Length} | binary: {binaryBuffer.Length}");
                var reader = new RawSourceWaveStream(binaryBuffer, 0, binaryBuffer.Length,
                    new WaveFormat(48000, 16, 1));
                using var convertedStream = WaveFormatConversionStream.CreatePcmStream(reader);
                var path = GenerateNewPath();
                WaveFileWriter.CreateWaveFile(path, convertedStream);
                return path;
            }
            public static string WavToOpus(string path, int bitrate = 64)
            {
                var outPath = path.Split('.')[0] + ".ogg";
                if (File.Exists(outPath)) File.Delete(outPath);

                var psi = new ProcessStartInfo
                {
                    FileName = "ThirdParty/opusenc.exe",
                    Arguments = $"--bitrate {bitrate} --quiet {path} {outPath}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                using (var process = Process.Start(psi))
                {
                    process?.StandardError.ReadToEnd();
                    process?.StandardOutput.ReadToEnd();
                }
                File.Delete(path);
#if deb
                Console.WriteLine("File ready: " + outPath);
#endif
                return outPath;
            }
            private static byte[] ToBinary(IReadOnlyList<short> arri, int offset, int lenSig)
            {
                var arrb = new byte[lenSig * 2 - offset * 2];
                var len = 0;
                for (var i = offset; i < lenSig; i++)
                {
                    var bytes = BitConverter.GetBytes(arri[i]);
                    arrb[len++] = bytes[0];
                    arrb[len++] = bytes[1];
                }

                return arrb;
            }
        }
        public class PreProcessor
        {
            private readonly Ema _ema1 = new Ema();
            public PreProcessor()
            {
                _ema1.Alpha = 0.9f;
            }

            public short[] ProcessTreshold(short[] inputshorts)
            {
                var start = 0; //Индекс подрезки
                var end = 0; //Индекс подрезки

                for (var i = 0; i < inputshorts.Length; i++)
                {
                    if (_ema1.Iterate(inputshorts[i]) < Configuration.Treshold) continue;
                    //Console.WriteLine("Max: "+inputshorts[i]);
                    start = i - Configuration.PreRecordBytes;
                    start = start > 0 ? start : 0;
                    break;
                } //прямой перебор для поиска восходящего фронта
                end = inputshorts.Length;

                var output = new short[end - start]; //вместилище результата
                var ctr2 = 0; //указатель для итеративной записи
                for (var i = start; i < end; i++) output[ctr2++] = inputshorts[i];
#if deb
                //диагностика
                Console.WriteLine($"Input: {inputshorts.Length}");
                Console.WriteLine($"Start: {start}");
                Console.WriteLine($"End: {end}");
                Console.WriteLine($"Output: {output.Length}");
#endif
                return output;
            }
        }
    }
}
