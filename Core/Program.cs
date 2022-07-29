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
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.FunctionalComponents;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2
{
    public static class Core
    {
        public static readonly AppConfiguration AppConfiguration = new("\\Configuration\\");///defaultConfig.env //Environment.CurrentDirectory+" / Configuration"
        public static readonly MorphAnalyzer MorphAnalyzer = new(true, true);
        public static readonly ProtocolsStorage ProtocolsStorage = new();//must be loaded
        public static readonly VariableStorage  GlobalVariables = new();//global variables storage
        private static readonly ModulesContainer Modules = new(/*new InnerDoubles<IModule>() { new SpeechModule(), new TestModule() }*/);
        
        private static void Main()
        {
            //ChachedSpeaker.ClearChache();
            
            //ProtocolsStorage.LoadProtocols();
            //ChachedSpeaker.Speak("Голосовая система загружена");
            //var protocol = RelevanceAnalyzer.GetRelevantProtocol(new SSpeechPattern("привет Кирилл"), ProtocolsStorage.Protocols);
            //Console.WriteLine($"THE MOST RELEVANT: {protocol.Name} -> {protocol.GetPattern()}");
            //ChachedSpeaker.Speak("Найден релевантный протокол");
            

            //Console.ReadLine();
            //return;

            ProtocolsStorage.LoadProtocols();
            Modules.Load(new List<Module>() {/*new SpeechModule(),*/new TestModule(),new Performer()});
            Modules.Start();
            MBus.Start();
            MBus.MakeRequest(new ModuleRequest("SRM","Performer","привет мир"));
            //MBus.MakeRequest(new ModuleRequest("Core","SRM","тестовое сообщение к SRM"));
            //MBus.MakeSpecialRequest("Performer SRM", MBus.SpecialRequestType.Subscribe);//subscribe test module to SRM messages
            //MBus.MakeSpecialRequest("test SRM", MBus.SpecialRequestType.Override);//subscribe test module to SRM messages for a once,then nobody will get this message except test-module
            Console.ReadKey();
        }
    }
}
