using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class SpeechPattern
    {
        public List<SpeechUnit> Units = new List<SpeechUnit>();
        public SpeechPattern(List<SpeechUnit> units)
        {
            Units = units;
        }
    }
}
