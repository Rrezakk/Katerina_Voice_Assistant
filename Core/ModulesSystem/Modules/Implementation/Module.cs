using System.Collections.Generic;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Implementation
{
    public abstract class Module
    {
        public string Name { get; protected set; }
        public ModuleBuffer InBuffer { get; set; } = new ModuleBuffer();
        private List<string> Subscribers { get; set; } = new List<string>();
        private SubscribeOverride OverrideS { get; set; }
        public abstract void Init();
        public abstract void Start();
        public void Subscribe(string moduleName)
        {
            Subscribers.Add(moduleName);
        }
        public void UnSubscribe(string moduleName)
        {
            Subscribers.Remove(moduleName);
        }
        public void Override(string moduleName, bool single = true)
        {
            OverrideS = new SubscribeOverride(moduleName, single);
        }
        public void SendRequest(string content)
        {
            if (OverrideS != null)
            {
                MBus.MakeRequest(new ModuleRequest(Name, OverrideS.Overrider, content));
                if (OverrideS.Single)
                    OverrideS = null;
                return;
            }
            foreach (var subscriber in Subscribers)
            {
                MBus.MakeRequest(new ModuleRequest(Name, subscriber, content));
            }
        }
    }
}
