﻿using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.ModulesImplementation;

namespace K3NA_Remastered_2.ModulesSystem.Modules
{
    public class ModulesContainer
    {
        private readonly List<IModule> _modules;
        public List<IModule> GetModules() => _modules;
        public ModulesContainer()
        {
            _modules = new List<IModule>();
        }
        public ModulesContainer(List<IModule> modules)
        {
            _modules = modules;
        }

        public void Load(List<IModule> modules)
        {
            _modules.AddRange(modules);
            foreach (var module in _modules)
            {
                try
                {
                    module.Init();
                    MBus.AuthModule(module);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Loading module error: {module.Name} : {e}");
                }
            }
        }
    }
}