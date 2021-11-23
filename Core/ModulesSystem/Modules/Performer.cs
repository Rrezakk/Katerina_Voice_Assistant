﻿using System;
using K3NA_Remastered_2.ModulesImplementation;
using K3NA_Remastered_2.ModulesSystem.Modules.Implementation;

namespace K3NA_Remastered_2.ModulesSystem.Modules
{
    class Performer:IModule
    {
        public string Name { get; set; } = "Performer";
        public ModuleBuffer InBuffer { get; set; } = new ModuleBuffer();
        public void Init()
        {
            InBuffer.OnRequest += OnRequestToModule;
            Console.WriteLine("Performer initialized");
        }

        private void OnRequestToModule(ModuleRequest request)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string moduleName)
        {
            throw new NotImplementedException();
        }

        public void UnSubscribe(string moduleName)
        {
            throw new NotImplementedException();
        }

        public void Override(string moduleName, bool single = true)
        {
            throw new NotImplementedException();
        }
    }
}
