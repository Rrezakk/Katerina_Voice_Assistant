using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols
{
    public abstract class Protocol
    {
        public abstract string Name { get; set; }
        //public abstract ProtocolType Type { get; set; }
        public abstract void Construct(string protocol);
        public abstract PSpeechPattern GetPattern();
        public abstract List<Command> GetCommands();
    }
}
