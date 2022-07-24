using System;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols.Loading
{
    internal class ProtocolsNetworkLoader:IProtocolsProvider
    {
        public string Url;
        public ProtocolsNetworkLoader(string url)
        {
            Url = url;
        }

        public string[] GetProtocols()
        {
            throw new NotImplementedException();
        }
    }
}
