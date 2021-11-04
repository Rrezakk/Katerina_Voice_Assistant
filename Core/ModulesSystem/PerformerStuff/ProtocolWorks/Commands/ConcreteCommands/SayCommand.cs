using System;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands
{
    class SayCommand:Command
    {
        public override string Name { get; set; }
        public override string[] Arguments { get; set; }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
