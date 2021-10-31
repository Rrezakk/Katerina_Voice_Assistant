using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class sSpeechPattern
    {
        public sSpeechPattern(string pattern)
        {
            var ptrn = pattern.Split(' ');
            foreach (var unit in ptrn)
            {
                AddUnit(new sSpeechUnit(unit));
            }
        }
        public List<sSpeechUnit> Units { get; } = new List<sSpeechUnit>();
        public void AddUnit(sSpeechUnit unit)
        {
            Units.Add(unit);
        }
        public string[] GetRawLemma()
        {
            string[] raw = new string[Units.Count];
            var index = 0;
            foreach (var unit in Units)
            {
                raw[index++] = unit.Lemma;
            }

            return raw;
        }
    }
}
