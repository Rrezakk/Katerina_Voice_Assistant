using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using DeepMorphy;
using K3NA_Remastered_2.Modules;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Variables;
using K3NA_Remastered_2.ModulesImplementation;

namespace K3NA_Remastered_2
{
    public static class Program
    {
        public static readonly MorphAnalyzer MorphAnalyzer = new MorphAnalyzer(true, true, true,4096);
        public static readonly ProtocolsStorage ProtocolsStorage = new ProtocolsStorage();//automaticaly loades protocols
        public static readonly VariableStorage  GlobalVariables = new VariableStorage();//global variables storage
        public static void OnSpeech(string speech)
        {
            var pattern = new sSpeechPattern(speech);
            var relevance = new double[ProtocolsStorage.Protocols.Count];
            var ptr = 0;
            foreach (var protocol in ProtocolsStorage.Protocols)//ProtocolsStorage foreach
            {
                relevance[ptr++] = RelevanceAnalyzer.GetRelevance(pattern, protocol.GetPattern());//GetRelevance
            }
            var bestProtoIndex = 0;
            var acc = 0d;
            for (var i = 0; i < relevance.Length; i++)//GetMaxRelevance
            {
                if (!(relevance[i] > acc)) continue;
                acc = relevance[i];
                bestProtoIndex = i;
            }
            var targetProtocol = ProtocolsStorage.Protocols[bestProtoIndex];//GetProtocolByMaxRelevance

            //filling variables
            //casting commands
            //enqueueing commands
        }

        public static readonly List<IModule> Modules = new List<IModule>();
        private static void LoadModules()
        {
            Modules.AddRange(new List<IModule>(){new SpeechModule(), new TestModule()});
            foreach (var module in Modules)
            {
                try
                {
                    module.Init();
                    MBus.AuthModule(module);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Loading module error: {module.Name} : {e}");
                }
            }
        }
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
                foreach (var unit in protocol.GetPattern().Units/*.Where(unit => unit.Morph != null)*/)
                {
                    if (unit.IsVariable)
                    {
                        Console.WriteLine($"    Variable: {unit.VariableName} : {unit.VariableTypeString}");
                        Console.WriteLine($"        {unit?.Morph?.Text} - {unit?.Morph?.BestTag}");
                    }
                    else
                    {
                        Console.WriteLine($"    Word: ");
                        Console.WriteLine($"        {unit?.Morph?.Text} - {unit?.Morph?.BestTag}");
                    }
                }
            }
        }
        private static string Str(this string[] a) => string.Join(';', a);
        private static void Main(string[] args)
        {
            DebugProtocols();

            //LoadModules();

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
