using System;
using K3NA_Remastered_2.Modules;
using K3NA_Remastered_2.Modules.PerformerStuff;
using K3NA_Remastered_2.ModulesImplementation;

namespace K3NA_Remastered_2
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //IModule srm = new SpeechModule();
            //srm.Init();
            //IModule test = new TestModule();
            //test.Init();

            //MBus.Start();

            //MBus.AuthModule(srm);
            //MBus.AuthModule(test);

            //MBus.MakeRequest(new ModuleRequest("test","SRM","тестовое сообщение"));
            //MBus.MakeSpecialRequest("test SRM",MBus.SpecialRequestType.Subscribe);//subscribe test module to SRM messages
            //MBus.MakeSpecialRequest("test SRM", MBus.SpecialRequestType.Override);//subscribe test module to SRM messages for a once, but nobody will get this message
            ProtoLoader.LoadProtocols();


            Console.ReadKey();
        }
      
    }
}
