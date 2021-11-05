using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Tables
{
    
        public static class ProtocolTypesTable
        {
            public static Protocol GetConcreteProtocol(string protocolName)
            {
                return protocolName switch
                {
                    "standard" => new DefaultProtocol(),
                    //"background" => ,
                    _ =>new UnknownProtocolType()
                };
            }
        }
    
}
