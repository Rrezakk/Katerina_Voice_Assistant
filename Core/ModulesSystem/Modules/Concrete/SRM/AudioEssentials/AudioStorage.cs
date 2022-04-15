using System.Collections.Generic;
using System.Linq;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
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
}