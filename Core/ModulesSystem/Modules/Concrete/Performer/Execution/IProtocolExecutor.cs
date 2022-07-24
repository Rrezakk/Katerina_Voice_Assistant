using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer.Execution
{
    internal interface IProtocolExecutor
    {
        public void Execute(Protocol protocol,List<Command> commands);
    }
}
