using System.Collections.Generic;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols.Loading;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Tables;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols
{
    public class ProtocolsStorage
    {
        private const string DefaultProtocolsPath = "StandardProtocols\\";
        public readonly List<Protocol> Protocols = new();
        public void LoadProtocols()
        {
            var providers = new List<IProtocolsProvider>() { new ProtocolsFileLoader(DefaultProtocolsPath)/*,new ProtocolsNetworkLoader()*/};
            foreach (var provider in providers)
            foreach (var protocol in provider.GetProtocols())
            {
                var type = ProtocolsParser.GetProtocolType(protocol);
                var proto = ProtocolTypesTable.GetConcreteProtocol(type);
                proto.Construct(protocol);
                Protocols.Add(proto);
            }
        }
    }
}
