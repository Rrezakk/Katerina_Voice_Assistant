using System;
using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer.Execution;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer
{
    internal sealed class Performer:Module
    {
        public static List<IProtocolExecutor> Executors = new() {new DefaultExecutor(),};
        public Performer()
        {
            this.Name = "Performer";
        }
        public override void Init()
        {
            InBuffer.OnRequest += OnRequestToModule;
            Console.WriteLine($">{Name} initialized");
        }
        public override void Start()
        {
            Console.WriteLine($">{Name} started");
        }
        private static void OnRequestToModule(ModuleRequest request)
        {
            switch (request.From)
            {
                case "SRM":
                    ProcessSrmRequest(request.Message);
                    break;
                case "test":

                    break;
            }
        }
        private static void ProcessSrmRequest(string speech)
        {
            if (string.IsNullOrEmpty(speech)) return;
            var recognizedSpeechPattern = new SSpeechPattern(speech);
            var protocol = RelevanceAnalyzer.GetMaxRelevanceProtocol(recognizedSpeechPattern, Core.ProtocolsStorage.Protocols);
            var protocolPattern = protocol.GetPattern();
            Console.WriteLine($"Protocol: {protocol.Name}");
            Console.WriteLine($"Pattern: {protocolPattern}");
            Console.WriteLine($"Speech: {recognizedSpeechPattern}");
            var storage = new VariableStorage(VariablesProcessor.ExtractVariables(protocolPattern, recognizedSpeechPattern));
            Console.WriteLine(storage);
            var commands = protocol.GetCommands();
            VariablesProcessor.FillArguments(storage, ref commands);
            if(Executors.Count>0)//choosing logic must be added
                Executors[0].Execute(protocol,commands);//заглушка до лучших времен
        }
    }
}
