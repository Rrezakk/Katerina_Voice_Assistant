using System;
using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using static K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols.ProtocolsParser;


namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols
{
    internal class DefaultProtocol:Protocol
    {
        public override string Name { get; set; }
        //public override ProtocolType Type { get; set; }
        private PSpeechPattern _pattern;
        private List<Command> _commands;
        public override void Construct(string protocol)
        {
            Console.WriteLine($"-----------------------------");
            Console.WriteLine($"Constructing DefaultProtocol");
            //var type = GetProtocolBlock(protocol, "Protocol:", "{");
            var pattern = GetProtocolBlock(protocol, "Pattern{", "}");
            var commands = GetProtocolBlock(protocol, "Commands{", "}");
            var name = GetProtocolBlock(protocol, "Name{", "}");
            Console.WriteLine($"Protocol name: {name}");
            Console.WriteLine($"Protocol pattern: {pattern}");
            Console.WriteLine($"Protocol commands: {commands}");
            Console.WriteLine("-----------------------------");
            _pattern = ParseProtocolPattern(pattern);
            _commands = ParseCommands(commands);
            Name = name;
        }
        public override PSpeechPattern GetPattern()
        {
            return _pattern;
        }

        public override List<Command> GetCommands()
        {
            return _commands;
        }
    }
}
