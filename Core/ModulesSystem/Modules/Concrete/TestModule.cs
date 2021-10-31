using System;
using K3NA_Remastered_2.ModulesImplementation;

namespace K3NA_Remastered_2.Modules
{
    class TestModule : IModule
    {
        public string Name { get; set; } = "test";
        public ModuleBuffer InBuffer { get ; set ; } = new ModuleBuffer();

        public void Init()
        {
            InBuffer.OnRequest += OnIncomingRequest;
            Console.WriteLine("test module initialized");
        }

        private void OnIncomingRequest(ModuleRequest request)
        {
            Console.WriteLine($"Request to {Name} from {request.From} : {request.Message}");
        }

        public void Start()
        {
        }

        public void Subscribe(string moduleName)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe(string moduleName)
        {
            throw new NotImplementedException();
        }

        public void Override(string moduleName, bool single = true)
        {
            throw new NotImplementedException();
        }
    }
}
