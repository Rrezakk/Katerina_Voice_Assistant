using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;

namespace K3NA_Remastered_2.ModulesSystem.Submodules
{
    public class SubModulesContainer
    {
        private readonly List<ISubModule> _subModules;
        public List<ISubModule> GetModules() => _subModules;
        public SubModulesContainer(List<ISubModule> smodules)
        {
            _subModules = smodules;
        }
        public void Init()
        {
            foreach (var sm in _subModules)
            {
                sm.Init();
            }
        }
        public CommandsExecutor CommandsExecutor() =>
            (CommandsExecutor) _subModules.First((m) => m.Name == "CommandsExecutor");
    }
}
