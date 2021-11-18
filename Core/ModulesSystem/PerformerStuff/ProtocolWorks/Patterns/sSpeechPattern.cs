using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns
{
    public class SSpeechPattern
    {
        public SSpeechPattern(string pattern)
        {
            var ptrn = pattern.Split(' ');
            foreach (var unit in ptrn)
            {
                AddUnit(new SSpeechUnit(unit));
            }
            Console.WriteLine(this);
        }
        public List<SSpeechUnit> Units { get; } = new List<SSpeechUnit>();
        public void AddUnit(SSpeechUnit unit)
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

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var unit in Units)
            {
                stringBuilder.Append("["+unit.ToString()+"] ");
            }

            return stringBuilder.ToString();
        }
    }
}
