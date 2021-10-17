using System;
using System.Linq;
using DeepMorphy;
using K3NA_Remastered_2.Modules;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesImplementation;

namespace K3NA_Remastered_2
{
    public static class Program
    {
        public static readonly MorphAnalyzer MorphAnalyzer = new MorphAnalyzer(true, true, true,4096);
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



            //var result = MorphAnalyzer.Parse("слово", "привет","закрой","ибрагим","очаровательно","она");
            //foreach (var r in result)
            //{
            //    Console.WriteLine($"{r.Text}: {r.BestTag}");
            //}
            //DefaultProtocol proto = new DefaultProtocol();
            //proto.Construct(ProtocolsLoader.GetProtocols().First());

            Console.ReadKey();
        }
      
    }
}
