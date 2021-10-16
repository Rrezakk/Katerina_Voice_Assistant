using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class pSpeechPattern
    {
        public List<pSpeechUnit> Units { get; } = new List<pSpeechUnit>();

        public void AddUnit(pSpeechUnit unit)
        {
            Units.Add(unit);
        }
    }
}
