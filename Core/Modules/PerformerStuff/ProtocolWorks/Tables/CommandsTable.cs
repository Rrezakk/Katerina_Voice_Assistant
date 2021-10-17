using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Tables
{
    public static class CommandsTable
    {
        public static Command GetConcreteCommand(string commandName)
        {
            return commandName switch
            {
                "Say" => new SayCommand(),
                "ConsolePrint" => new ConsolePrintCommand(),
                _ => new NotRecognizedCommand()
            };
        }
    }
}
