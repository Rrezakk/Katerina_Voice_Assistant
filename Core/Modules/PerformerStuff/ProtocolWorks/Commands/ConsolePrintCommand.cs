using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands
{
    public class ConsolePrintCommand:Command
    {
        public override void Execute(object[] arguments)
        {
            try
            {
                Console.WriteLine(arguments[0].ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine($"ConsolePrintCommand error: {e}");
                throw;
            }
        }
    }
}
