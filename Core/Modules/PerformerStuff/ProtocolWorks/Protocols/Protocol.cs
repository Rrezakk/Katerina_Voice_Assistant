using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    public abstract class Protocol
    {
        public enum ProtocolTypes
        {
            Default,
            Background,
            
        }
        public abstract void Construct(string protocol);
        public abstract SpeechPattern GetPattern();
    }
}
