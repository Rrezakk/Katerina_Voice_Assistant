using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using K3NA_Remastered_2.ModulesSystem.Submodules;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands
{
    public class CommandsExecutor: ISubModule
    {
        private static int _indexPtr = 0;//can cause errors when reached max
        private delegate void OnNew(int index);
        private event OnNew OnNewElement;
        private readonly Dictionary<int,CommandsContainer> _commandsContainers = new Dictionary<int, CommandsContainer>();
        private readonly object _containersLocker = new object();
        public string Name { get; set; } = "CommandsExecutor";
        public void Init()
        {
            OnNewElement += CreateExecutingThread;
        }
        public void EnqueueNew(CommandsContainer commands)
        {
            var index = 0;
            lock (_containersLocker)
            {
                index = _indexPtr++;
                _commandsContainers.Add(index, commands);
            }
            Console.WriteLine($"Enqueued commandsContainer at {index} -> Parent:{commands.ParentProtocolName};Count:{commands.Commands.Count}");
            foreach (var command in commands.Commands)
            {
                Command.About(command);
            }
            OnNewElement?.Invoke(index);
        }
        private async void CreateExecutingThread(int index)
        {
            await Task.Run(() => Execute(index));
        }
        private void Execute(int index)
        {
            lock (_containersLocker)
            {
                var container = _commandsContainers[index];
                //logic
                throw new NotImplementedException();
                _commandsContainers.Remove(index);
            }
        }
    }
}
