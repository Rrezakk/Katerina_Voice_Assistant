using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.Submodules
{
    public interface ISubModule
    {
        public string Name { get; set; }
        public void Init();
    }
}
