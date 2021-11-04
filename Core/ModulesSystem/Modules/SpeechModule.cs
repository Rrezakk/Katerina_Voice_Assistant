using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using K3NA_Remastered_2.ModulesImplementation;
using k3na_voice;
using ModuleBuffer = K3NA_Remastered_2.ModulesSystem.Modules.Implementation.ModuleBuffer;
using ModuleRequest = K3NA_Remastered_2.ModulesImplementation.ModuleRequest;

namespace K3NA_Remastered_2.ModulesSystem.Modules
{
    
    internal sealed class SpeechModule:IModule
    {
        private class SubscribeOverride
        {
            public SubscribeOverride(string overrider,bool single=true)
            {
                this.Overrider = overrider;
                this.Single = single;
            }
            public readonly string Overrider;
            public readonly bool Single;
        }
        public SpeechModule()
        {
            Console.WriteLine("SRM constructed");
        }
        public string Name { get; set; } = "SRM";
        public ModuleBuffer InBuffer { get; set; } = new ModuleBuffer();
        private readonly List<string> _subscribers = new List<string>();
        private SpeechRecognitionModule _srm;
        private SubscribeOverride _override;
        public async void Init()
        {
            await Task.Run(InitTask);
        }
        public void Start()
        {
            throw new NotImplementedException();
        }
        public void Subscribe(string moduleName)
        {
            _subscribers.Add(moduleName);
        }
        public void UnSubscribe(string moduleName)
        {
            _subscribers.Remove(moduleName);
        }

        public void Override(string moduleName, bool single = true)
        {
            _override = new SubscribeOverride(moduleName,single);
        }

        private void InitTask()
        {
            Console.WriteLine("SRM initialized");
            InBuffer.OnRequest += OnIncomingRequest;
            try
            {
                _srm = new SpeechRecognitionModule();
                _srm.OnSpeech += Module_OnSpeech;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Module init exception: {Name} - {e}");
            }
        }
        private static void OnIncomingRequest(ModuleRequest request)
        {
            Console.WriteLine($"New request from:{request.From} To:{request.To} ::: {request.Message}");
        }
        private void Module_OnSpeech(string speech)
        {
            if (_override != null)
            {
                MBus.MakeRequest(new ModuleRequest(Name,_override.Overrider,speech));
                if (_override.Single)
                    _override = null;
                return;
            }
            foreach (var subscriber in _subscribers)
            {
                MBus.MakeRequest(new ModuleRequest(Name,subscriber,speech));
            }
        }
    }
}
