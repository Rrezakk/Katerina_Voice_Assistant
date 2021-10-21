using System;
using System.Diagnostics.Eventing.Reader;
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
        private static readonly ProtocolsStorage ProtocolsStorage = new ProtocolsStorage();
        private static void DebugProtocols()
        {
            foreach (var protocol in ProtocolsStorage.Protocols)
            {
                Console.WriteLine($"Loaded protocol: {protocol.Name}");
                Console.WriteLine($"Commands: ");
                foreach (var c in protocol.GetCommands())
                {
                    Console.WriteLine($"Command: {c.Name}");
                    var args = c.Arguments.Str();
                    if(args!="")
                        Console.WriteLine($"Arguments: {args}");
                }
                Console.WriteLine("Pattern: ");
                foreach (var unit in protocol.GetPattern().Units.Where(unit => unit.MorphList != null))
                {
                    if (unit.IsVariable)
                    {
                        Console.WriteLine($"    Variable: {unit.VariableName}");
                        foreach (var morph in unit.MorphList)
                        {
                            Console.WriteLine($"        {morph.Text} - {morph.BestTag}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"    Word: ");
                        foreach (var morph in unit.MorphList)
                        {
                            Console.WriteLine($"        {morph.Text} - {morph.BestTag}");
                        }
                    }
                }
            }
        }
        private static string Str(this string[] a) => string.Join(';', a);
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

            DebugProtocols();



            Console.ReadKey();
        }
      
    }
}
