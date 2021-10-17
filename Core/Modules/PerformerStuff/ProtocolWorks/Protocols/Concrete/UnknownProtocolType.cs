using System;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    internal class UnknownProtocolType:Protocol
    {
        public override void Construct(string protocol)
        {
            Console.WriteLine($"Unknown protocol constructing: {protocol}");
        }

        public override pSpeechPattern GetPattern()
        {
            return null;
        }
    }
}
