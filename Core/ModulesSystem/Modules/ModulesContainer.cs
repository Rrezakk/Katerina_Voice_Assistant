using System;
using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;

namespace K3NA_Remastered_2.ModulesSystem.Modules
{
    public class ModulesContainer
    {
        private readonly List<Module> _modules;
        public List<Module> GetModules() => _modules;
        public ModulesContainer()
        {
            _modules = new List<Module>();
        }
        public ModulesContainer(List<Module> modules)
        {
            _modules = modules;
        }
        public void Load(IEnumerable<Module> modules)
        {
            _modules.AddRange(modules);
            foreach (var module in _modules)
            {
                try
                {
                    module.Init();
                    MBus.AuthModule(module);
                    Console.WriteLine($"Module loaded: {module.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Loading module error: {module.Name} : {e}");
                }
            }
        }
        public void Start()
        {
            foreach (var module in _modules)
            {
                try
                {
                    module.Start();
                    Console.WriteLine($"Module started: {module.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Start module error: {module.Name} : {e}");
                }
            }
        }
    }
}
