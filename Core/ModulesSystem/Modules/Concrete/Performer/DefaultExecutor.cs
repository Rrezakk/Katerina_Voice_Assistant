using System.Collections.Generic;
using System.Threading.Tasks;
using Colorful;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer
{
    public class DefaultExecutor
    {
        
        public void Execute(List<Command> commands)
        {
            Task.Run(() =>
            {
                foreach (var command in commands)
                {
                    Console.WriteLine($"Trying executing: { command.Name}");
                    command.Execute();
                }
            });
        }

    }
}
