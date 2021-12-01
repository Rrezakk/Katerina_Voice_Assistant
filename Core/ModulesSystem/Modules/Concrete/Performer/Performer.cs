using System;
using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer
{
    internal sealed class Performer:Module
    {
        //private ExecutionForwarder _executionForwarder = new ExecutionForwarder();
        private DefaultExecutor _executor = new DefaultExecutor();//заглушка до лучших времен
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
        private static void ProcessSrmRequest(string speech)
        {
            var speechPattern = new SSpeechPattern(speech);
            var protocol = RelevanceAnalyzer.GetMaxRelevanceProtocol(speechPattern, Core.ProtocolsStorage.Protocols);
            var protocolPattern = protocol.GetPattern();
            Console.WriteLine($"Protocol: {protocol.Name}");
            Console.WriteLine($"Pattern: {protocolPattern}");
            Console.WriteLine($"Speech: {speechPattern}");
            var storage = new VariableStorage(VariablesProcessor.ExtractVariables(protocolPattern, speechPattern));
            Console.WriteLine(storage);
            var commands = protocol.GetCommands();
            VariablesProcessor.FillArguments(storage, ref commands);
            //_executor.Execute();
        }
        private static void OnRequestToModule(ModuleRequest request)
        {
            switch (request.From)
            {
                case "SRM":
                    ModuleRequestParser.ParseSrmRequest(request.Message,out string speech);
                    ProcessSrmRequest(speech);
                    break;
                case "test":

                    break;
            }
        }
    }
}
