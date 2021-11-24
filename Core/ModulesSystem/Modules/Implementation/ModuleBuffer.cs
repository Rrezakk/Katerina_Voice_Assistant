using System.Collections.Generic;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Implementation
{
    public class ModuleBuffer
    {
        private readonly object _requestsLocker = new object();
        private Queue<ModuleRequest> _requests = new Queue<ModuleRequest>();
        public delegate void RequestDelegate(ModuleRequest request);

        public event RequestDelegate OnRequest;

        private int _requestsCounter = 0;
        public void Put(ModuleRequest request)
        {
            lock (_requestsLocker)
            {
                _requests.Enqueue(request);
                OnRequest?.Invoke(request);
                _requestsCounter++;
                if (_requestsCounter>10)
                {
                    _requests = new Queue<ModuleRequest>();
                }
            }
        }
    }
}
