using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Tables
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
