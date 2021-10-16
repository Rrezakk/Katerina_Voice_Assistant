using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    class sSpeechPattern
    {
        public List<sSpeechUnit> Units { get; } = new List<sSpeechUnit>();

        public void AddUnit(sSpeechUnit unit)
        {
            Units.Add(unit);
        }
    }
}
