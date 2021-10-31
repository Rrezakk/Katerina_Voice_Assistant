using System;

namespace K3NA_Remastered_2.ModulesImplementation
{
    public class ModuleRequest:ICloneable
    {
        
        public ModuleRequest() {}
        public ModuleRequest(string from, string to,string message)
        {
            this.To = to;
            this.From = from;
            this.Message = message;
        }
        private static long _generateId() => _nextId++;
        private static long _nextId = 0;

        public readonly string From;
        public readonly string To;
        public readonly string Message;
        public readonly long Id = _generateId();
        public object Clone()
        {
            return new ModuleRequest(this.From,this.To,this.Message) {};
        }
    }
}