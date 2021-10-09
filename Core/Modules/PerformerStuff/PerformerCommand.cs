using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.Modules.PerformerStuff
{
    public abstract class PerformerCommand
    {
        public Task Task { get; set; }
        public abstract void Perform();

    }
    public class DefaultPerformerCommand:PerformerCommand
    {
        public new Task Task { get; set; }
        public DefaultPerformerCommand()
        {
            Task = Task.CompletedTask;//what?
        }
        public override void Perform()
        {
            Task.Start();//unsafe
        }

        public void Perform(Task t)
        {
            this.Task = t;
            Perform();
        }

    }
}
