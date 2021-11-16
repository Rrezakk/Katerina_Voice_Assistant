using System;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands
{
    public class NotRecognizedCommand:Command
    {
        public override string Name { get; set; }
        public override string[] Arguments { get; set; }
        public override void Execute()
        {
            string args = String.Join(';', Arguments);
            Console.WriteLine($"Executed non-recognized command with arguments: {args}");
        }
    }
}
