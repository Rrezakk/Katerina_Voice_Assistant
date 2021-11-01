using System;
using System.Collections.Generic;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    internal class UnknownProtocolType:Protocol
    {
        public override string Name { get; set; }

        public override void Construct(string protocol)
        {
            Console.WriteLine($"Unknown protocol constructing: {protocol}");
            Name = "Unknown";
        }

        public override PSpeechPattern GetPattern()
        {
            return new PSpeechPattern();
        }

        public override List<Command> GetCommands()
        {
            return new List<Command>();
        }
    }
}
