using System;

namespace k3na_voice
{
    public class ModuleRequest:ICloneable
    {
        public object Clone()
        {
            return new ModuleRequest(){};
        }
    }
}