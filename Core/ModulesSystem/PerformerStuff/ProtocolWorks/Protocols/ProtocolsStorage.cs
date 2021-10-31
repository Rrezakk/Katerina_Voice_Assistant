using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Tables;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    public class ProtocolsStorage
    {
        public readonly List<Protocol> Protocols = new List<Protocol>();
        public ProtocolsStorage()
        {
            foreach (var protocol in ProtocolsLoader.GetProtocols())
            {
                var type = ProtocolsParser.GetProtocolType(protocol);
                var proto = ProtocolTypesTable.GetConcreteProtocol(type);
                proto.Construct(protocol);
                Protocols.Add(proto);
            }
        }

    }
}
