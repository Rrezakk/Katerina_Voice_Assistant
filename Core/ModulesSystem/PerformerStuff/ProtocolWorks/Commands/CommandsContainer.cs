using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands
{
    public class CommandsContainer
    {
        public List<Command> Commands;
        public VariableStorage VariableStorage;
        public string ParentProtocolName;
        public CommandsContainer(List<Command> commands, VariableStorage vars, string parentProtocolName="unknown")
        {
            this.Commands = commands;
            this.VariableStorage = vars;
            this.ParentProtocolName = parentProtocolName;
        }
    }
}
