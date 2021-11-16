using System;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands
{
    public class ConsolePrintCommand:Command
    {
        public override string Name { get; set; }
        public override string[] Arguments { get; set; }
        private bool Check()
        {
            return Arguments.Length == 1;
        }
        public override void Execute()
        {
            try
            {
                if (Check())
                {
                    Console.WriteLine(Arguments[0]);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Arguments","was not 1 length to ConsolePrint");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"ConsolePrintCommand error: {e}");
                throw;
            }
        }
    }
}
