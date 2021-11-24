using NAudio.Wave;
using static K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials;
//#define deb

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM
{
    public class Audist
    {
        public Audist()
        {
            _storage.AmplitudeEma.Alpha = Configuration.BasicEmaAlpha;
            OnFragment += Audist_onFragment;
        }
        private WaveInEvent _waveSource;

        public delegate void OnFragmentDelegate(int id);

        public delegate void OnAudioFileDelegate(string path);

        public event OnFragmentDelegate OnFragment;
        public event OnAudioFileDelegate OnAudioFile;


        private AudioFragment _fragment = new AudioFragment();
        private AudioStorage _storage = new AudioStorage();
        private PreProcessor _preProcessor = new PreProcessor();
        private AudioFileProcessor _audioFileProcessor = new AudioFileProcessor();

        private bool _previouslyIterated;
        private void WaveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (!_fragment.IsFull())
            {
                _fragment.AppendBytes(e.Buffer);
                return;
            }

            _fragment.Process();

            if (_storage.AmplitudeEma.Iterate(_fragment.MaxAmplitude) >= Configuration.Treshold + NoiseMeter.GetVolume())//Experimental
            {
                _storage.Add(_fragment);
                _previouslyIterated = true;
            }
            else
            {
                if (_previouslyIterated)
                {
                    _storage.Add(_fragment);
                    OnFragment?.Invoke(_storage.Count()); //Подняв событие, мы получаем id первого блока, не проходящего treshold
                    _previouslyIterated = false;
                }
            }

            _fragment.Clear();
            _fragment.AppendBytes(e.Buffer);
        }
        private void Audist_onFragment(int id)
        {
            //записываем номерок, считаем по postbytes, сколько нужно ещё записать фрагментов.
            var audio = _preProcessor.ProcessTreshold(_storage.GetSolidWr(id));
            var path = AudioFileProcessor.SaveWav(audio);
            path = AudioFileProcessor.WavToOpus(path);
            OnAudioFile?.Invoke(path);
        }
        public void Start()
        {
            _waveSource = new WaveInEvent
            {
                WaveFormat = new WaveFormat(Configuration.SampleRate, Configuration.BitResolution, Configuration.Channels),
                DeviceNumber = Configuration.SoundDevice,
                BufferMilliseconds = Configuration.WaveInputBuffer
            };
            _waveSource.DataAvailable += WaveSource_DataAvailable;
            _waveSource.RecordingStopped += WaveSource_RecordingStopped;
            _waveSource.StartRecording();
        }
        public void Stop()
        {
            _waveSource.StopRecording();
        }
        private void WaveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (_waveSource == null) return;
            _waveSource.Dispose();
            _waveSource = null;
        }
    }
}