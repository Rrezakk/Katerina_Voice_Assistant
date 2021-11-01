using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class PSpeechPattern
    {
        public List<PSpeechUnit> Units { get; } = new List<PSpeechUnit>();

        public void AddUnit(PSpeechUnit unit)
        {
            Units.Add(unit);
        }

        public string[] GetLemmaArray()
        {
            var ans = new string[Units.Count];
            var ptr = 0;
            foreach (var unit in Units)
            {
                ans[ptr++] = unit?.Morph?.First()?.BestTag?.Lemma;
            }
            return ans;
        }
    }
}
