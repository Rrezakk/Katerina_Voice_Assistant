using System;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands
{
    public class CommandsExecutor
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void EnqueueNew(CommandsContainer commands)
        {
            //throw new NotImplementedException();
            Console.WriteLine($"Enqueued commandsContainer: parent:{commands.ParentProtocolName} count:{commands.Commands.Count}");
            foreach (var command in commands.Commands)
            {
                Command.About(command);
            }
        }
    }
}
