#define dia //for diagnostics
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using NAudio.Wave;

namespace k3na_voice
{
    public static class AudioEssentials
    {
        public class Ema
        {
            private readonly object _emaLocker = new object();
            private int _previous = 1;

            public float
                Alpha = Configuration.DefaultEmaAlpha; //α – весовой коэффициент в интервале от 0 до 1, отражающий скорость старения прошлых данных:

            //чем выше его значение, тем больший удельный вес имеют новые наблюдения случайной величины, и тем меньший старые;
            public int Iterate(int value)
            {
                lock (_emaLocker)
                {
                    _previous = (int)(Alpha * Math.Abs(value) + (1 - Alpha) * _previous);
                    //Console.WriteLine(previous);
                    return _previous /*_previous*/;
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
            private static readonly Ema Ema = new Ema();
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
            private int _audioShortsPtr;
            private int _ptr;
            public int MaxAmplitude;
            public int TotalVolume;

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
                foreach (var amp in _audioShorts)
                {
                    try
                    {
                        if (MaxAmplitude < Math.Abs(amp)) MaxAmplitude = Math.Abs(amp); //Максимальная громкость звука
                        TotalVolume += amp;
                    }catch (Exception e){}
                }

                if (MaxAmplitude < Configuration.Treshold)//Experimental
                {
                    NoiseMeter.TakeMeasure(MaxAmplitude);
                }
            }

            public byte[] GetBytes()
            {
                return _audioBytes;
            }

            public short[] GetShorts()
            {
                return _audioShorts;
            }

            public void Clear()
            {
                _audioBytes = new byte[48000];
                _ptr = 0;
                MaxAmplitude = 0;
                TotalVolume = 0;
                _audioShorts = new short[24000];
                _audioShortsPtr = 0;
            }

            public object Clone()
            {
                return new AudioFragment
                {
                    _audioBytes = _audioBytes,
                    MaxAmplitude = MaxAmplitude,
                    _ptr = _ptr,
                    TotalVolume = TotalVolume,
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
#if dia
Console.WriteLine($"Fragment {_fragments.Count}: {fragment.MaxAmplitude}/{Configuration.Treshold}");
#endif
                    
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

            public byte[] GetSolid()
            {
                lock (_fragmentsLocker)
                {
                    var solid = new byte[_fragments.Count * 48000];
                    var ptr = 0;
                    foreach (var audioFragment in _fragments)
                    {
                        var fragmentBytes = audioFragment.GetBytes();
                        foreach (var t in fragmentBytes) solid[ptr++] = t;
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

        public class AudioFileProcessor
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
                    FileName = "opusenc.exe",
                    Arguments = $"--bitrate {bitrate} --quiet {path} {outPath}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var errors = "";
                var results = "";
                using (var process = Process.Start(psi))
                {
                    errors = process.StandardError.ReadToEnd();
                    results = process.StandardOutput.ReadToEnd();
                }

                File.Delete(path);
#if deb
                Console.WriteLine("File ready: " + outPath);
#endif
                return outPath;
            }
        }

        public class PreProcessor
        {
            private readonly Ema _ema1 = new Ema();

            //private Ema _ema2 = new Ema();
            public PreProcessor()
            {
                _ema1.Alpha = 0.9f;
                //_ema2.Alpha = 1f;
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
        public static byte[] ToBinary(IReadOnlyList<short> arri, int offset, int lenSig)
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
}
