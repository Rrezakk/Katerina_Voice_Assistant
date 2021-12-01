using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.Performer
{
    public class ExecutionForwarder
    {
        public ExecutionForwarder()
        {
            this._devices.Add(new Device("core",DeviceType.Core));
        }
        private List<Device> _devices = new List<Device>();
        public class Device
        {
            public Device(string name,DeviceType type)
            {
                this.Name = name;
                this.Type = type;
            }
            public string Name;
            public DeviceType Type;
        }
        public enum DeviceType
        {
            Core,
            Laptop,
            PC,
            Smartphone,
            Tablet,
            Car,
        }
        public enum DeviceConnectionType
        {
            Tcp,
            Mbus,

        }
        public void Execute(string deviceName = "core")
        {
            foreach (var device in _devices)
            {
                if (device.Name== deviceName)
                {
                    
                }
            }
        }
    }
}
