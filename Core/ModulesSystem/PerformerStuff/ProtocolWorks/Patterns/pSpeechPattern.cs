using System;
using System.Collections.Generic;
using System.Linq;
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

        public string[] GetLemmaArray()
        {
            var ans = new string[Units.Count];
            var ptr = 0;
            foreach (var unit in Units)
            {
                ans[ptr++] = unit.Morph.BestTag.Lemma;
            }
            return ans;
        }
    }
}
