using System;
using System.Collections.Generic;
using System.Text;
using NAudio.CoreAudioApi;

namespace k3na_voice
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ModuleBuffer
    {
        private readonly object _bufferLocker = new object();
        private List<ModuleRequest> _requests = new List<ModuleRequest>();
        public void Add(ModuleRequest value)
        {
            lock (_bufferLocker)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _requests.Add(value);
            }
        }
        public List<ModuleRequest> ExtractWr()
        {
            lock (_bufferLocker)
            {
                var tmp = new List<ModuleRequest>();
                foreach (var request in _requests)
                {
                    tmp.Add((ModuleRequest) request.Clone());
                }

                _requests = new List<ModuleRequest>();
                return tmp;
            }
        }
        public bool CanSeek()
        {
            lock (_bufferLocker)
            {
                return _requests.Count - 1 >= 0;
            }
        }
        public ModuleRequest Seek()
        {
            lock (_bufferLocker)
            {
                try
                {
                    var ans = (ModuleRequest)_requests[^1].Clone();
                    _requests.RemoveAt(_requests.Count - 1);
                    return ans;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
