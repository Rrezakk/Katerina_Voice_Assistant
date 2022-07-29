using System;
using System.Linq;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; set; }
        public abstract string[] Arguments { get; set; }
        public abstract void Execute();
        public static void About(Command c)
        {
            Console.WriteLine($"Command: {c.Name} Args: {JoinArgs(c.Arguments)}");
        }
        private static string JoinArgs(string[] args)
        {
            return args.Aggregate("", (current, a) => current + $"[{a}] ");
        }
    }
}
