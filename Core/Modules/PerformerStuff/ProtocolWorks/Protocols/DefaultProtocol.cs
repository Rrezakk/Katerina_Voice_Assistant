using System;
using System.Collections.Generic;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using static K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols.ProtocolsParser;


namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    class DefaultProtocol:Protocol
    {
        public string Name;
        public SpeechPattern Pattern;
        public List<Command> Commands;
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
            Pattern = ParsePattern(pattern);
            Commands = ParseCommands(commands);
            Name = name;
        }
        public override SpeechPattern GetPattern()
        {
            return Pattern;
        }
    }
}
