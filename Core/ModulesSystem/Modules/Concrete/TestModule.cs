using System;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete
{
    internal sealed class TestModule : Module
    {
        public TestModule()
        {
            this.Name = "test";
        }
        public override void Init()
        {
            InBuffer.OnRequest += OnIncomingRequest;
            Console.WriteLine("test module initialized");
        }
        public override void Start()
        {
            //nothing but that's allright
        }
        private void OnIncomingRequest(ModuleRequest request)
        {
            Console.WriteLine($"Request to {Name} from {request.From} : {request.Message}");
        }
    }
}
