using System;
using System.Collections.Generic;
using DeepMorphy;
using K3NA_Remastered_2.Configuration;
using K3NA_Remastered_2.LanguageExtensions;
using K3NA_Remastered_2.ModulesSystem.Modules;
using K3NA_Remastered_2.ModulesSystem.Modules.Concrete;
using K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer;
using K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2
{
    public static class Core
    {
        public static readonly AppConfiguration AppConfiguration = new AppConfiguration("\\Configuration\\");///defaultConfig.env //Environment.CurrentDirectory+" / Configuration"
        public static readonly MorphAnalyzer MorphAnalyzer = new MorphAnalyzer(true, true);
        public static readonly ProtocolsStorage ProtocolsStorage = new ProtocolsStorage();//automaticaly loades protocols
        public static readonly VariableStorage  GlobalVariables = new VariableStorage();//global variables storage
        private static readonly ModulesContainer Modules = new ModulesContainer(/*new InnerDoubles<IModule>() { new SpeechModule(), new TestModule() }*/);
        private static void Test()
        {
            var phrases = new List<string>() { "Алёнка и другие и я",/*,"найди кокос","привет тебе пупсик"*//*,"привет обыватель","найди","привет тебе цветок"*/};
            foreach (var phrase in phrases)
            {
                OnSpeech(phrase);
            }
        }
        private static void OnSpeech(string speech)
        {
            var speechPattern = new SSpeechPattern(speech);
            var protocol = RelevanceAnalyzer.GetMaxRelevanceProtocol(speechPattern, ProtocolsStorage.Protocols);
            var protocolPattern = protocol.GetPattern();
            Console.WriteLine($"Protocol: {protocol.Name}");
            Console.WriteLine($"Pattern: {protocolPattern}");
            Console.WriteLine($"Speech: {speechPattern}");
            var storage = new VariableStorage(VariablesProcessor.ExtractVariables(protocolPattern, speechPattern));
            Console.WriteLine(storage);
            var commands = protocol.GetCommands();
            VariablesProcessor.FillArguments(storage,ref commands);

            //storage.ExecuteRecognizedProtocol(proto, speechPattern);//конечная точка в обработке
        }
        private static void DebugProtocols()
        {
            foreach (var protocol in ProtocolsStorage.Protocols)
            {
                Console.WriteLine($"Loaded protocol: {protocol.Name}");
                Console.WriteLine("Commands: ");
                foreach (var c in protocol.GetCommands())
                {
                    Console.WriteLine($"    Command: {c.Name}");
                    var args = c.Arguments.Concat();
                    if(args!="")
                        Console.WriteLine($"        Arguments: {args}");
                }
                Console.WriteLine("Pattern: ");
                foreach (var unit in protocol.GetPattern().Units/*.Where(unit => unit.Morph != null)*/)
                {
                    if (unit.IsVariable)
                    {
                        Console.WriteLine($"    Variable: {unit.VariableName} : {unit.TypeString}");
                        if(unit.Morph==null)continue;
                        foreach (var morph in unit.Morph)
                        {
                            Console.WriteLine($"        {morph?.Text} - {morph?.BestTag}");
                        }
                    }
                    else
                    {
                        if (unit.Morph == null) continue;
                        Console.WriteLine($"    Word [{unit.TypeString}]: ");
                        foreach (var morph in unit.Morph)
                        {
                            Console.WriteLine($"        {morph?.Text} - {morph?.BestTag}");
                        }
                    }
                }
            }
        }
        private static void Main()
        {
            //Test();
            //DebugProtocols();
            Modules.Load(new List<Module>() {new SpeechModule(),new TestModule(),new Performer()});
            Modules.Start();
            MBus.Start();
            //MBus.MakeRequest(new ModuleRequest("SRM","Performer","привет мир"));
            //MBus.MakeRequest(new ModuleRequest("Core","SRM","тестовое сообщение к SRM"));
            MBus.MakeSpecialRequest("Performer SRM", MBus.SpecialRequestType.Subscribe);//subscribe test module to SRM messages
            //MBus.MakeSpecialRequest("test SRM", MBus.SpecialRequestType.Override);//subscribe test module to SRM messages for a once,then nobody will get this message except test-module
            Console.ReadKey();
        }
    }
}
