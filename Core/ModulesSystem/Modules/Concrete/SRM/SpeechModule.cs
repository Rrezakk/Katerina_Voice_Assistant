using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;
using ModuleBuffer = K3NA_Remastered_2.ModulesSystem.Modules.Implementation.ModuleBuffer;
using ModuleRequest = K3NA_Remastered_2.ModulesSystem.Modules.Implementation.ModuleRequest;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM
{
    internal sealed class SpeechModule:Module
    {
        public SpeechModule()
        {
            this.Name = "SRM";
            Console.WriteLine("SRM constructed");
        }
        private SpeechRecognizer _srm;
        public override async void Init()
        {
            await Task.Run(() =>
            {
                InBuffer.OnRequest += OnIncomingRequest;
                try
                {
                    _srm = new SpeechRecognizer();
                    _srm.OnSpeech += SpeechRecognizer_OnSpeech;
                    Console.WriteLine($">{Name} initialized");
                }
                catch (Exception e)
                {
                    Console.WriteLine($">Module init exception: {Name} - {e}");
                }
            });
        }
        public override void Start()
        {
            _srm.Start();
        }
        private static void OnIncomingRequest(ModuleRequest request)
        {
            Console.WriteLine($"New request from:{request.From} To:{request.To} ::: {request.Message}");
        }
        private void SpeechRecognizer_OnSpeech(string speech)
        {
            base.SendRequest(speech);
        }
    }
}
