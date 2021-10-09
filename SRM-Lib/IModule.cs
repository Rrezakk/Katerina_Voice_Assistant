using System;
using System.Collections.Generic;
using System.Text;

namespace k3na_voice
{
    interface IModule
    {
        public string Name { get; set; }
        public ModuleBuffer InBuffer { get; set; }
        public ModuleBuffer OutBuffer { get; set; }
        public void Init(string[] param);
        public void Run();

    }
}
