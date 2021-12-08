using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands
{
    class ExecuteCommand:Command
    {
        private static List<Task> launchedTasks = new List<Task>();
        public override string Name { get; set; }
        public override string[] Arguments { get; set; }
        public override void Execute()
        {
            if (Arguments?.Length>0)
            {
                Action t = new Action(() =>
                {

                });
                launchedTasks.Add(Task.Run(t));
            }
        }
    }
}
