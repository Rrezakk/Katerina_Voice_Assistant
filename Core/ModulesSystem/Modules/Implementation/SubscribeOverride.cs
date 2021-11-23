using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Implementation
{
    public class SubscribeOverride
    {
        public SubscribeOverride(string overrider, bool single = true)
        {
            this.Overrider = overrider;
            this.Single = single;
        }
        public readonly string Overrider;
        public readonly bool Single;
    }
}
