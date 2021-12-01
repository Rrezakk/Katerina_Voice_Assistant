using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer
{
    public class DefaultExecutor
    {
        private readonly object _commandsLocker = new object();
        private readonly Queue<List<Command>> _commands = new Queue<List<Command>>();
        public void EnqueueCommands(List<Command> commands)
        {
            lock (_commandsLocker)
            {
                _commands.Enqueue(commands);
            }
        }
        public void Execute()
        {
            Task.Run(() =>
            {
                List<Command> commands;
                lock (_commandsLocker)
                {
                    if (!_commands.TryDequeue(out commands)) return;
                }
                foreach (var command in commands)
                {
                    command.Execute();
                }
            });
        }
    }
}
