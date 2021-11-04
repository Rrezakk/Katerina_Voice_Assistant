using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;

namespace K3NA_Remastered_2.ModulesImplementation
{
    public interface IModule
    {
        public string Name { get; set; }
        public ModuleBuffer InBuffer { get; set; }
        public void Init();
        public void Start();
        public void Subscribe(string moduleName);
        public void UnSubscribe(string moduleName);
        public void Override(string moduleName, bool single = true);
    }
}
