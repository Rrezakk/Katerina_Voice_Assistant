using System.Collections.Generic;
using System.Threading.Tasks;
using Colorful;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer.Execution
{
    public class DefaultExecutor:IProtocolExecutor
    {

        public void Execute(Protocol protocol, List<Command> commands)
        {
            Task.Run(() =>
            {
                foreach (var command in commands)
                {
                    Console.WriteLine($"Trying executing: {command.Name}");
                    command.Execute();
                }
            });
        }
    }
}
