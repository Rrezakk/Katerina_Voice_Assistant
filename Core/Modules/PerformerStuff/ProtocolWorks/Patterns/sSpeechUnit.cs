using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepMorphy.Model;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class sSpeechUnit
    {
        public readonly MorphInfo MorphInfo;
        public string Text;
        public string Lemma;
        public sSpeechUnit()
        {
            MorphInfo = null;
            Text = "";
            Lemma = "";
        }
        public sSpeechUnit(string unit)
        {
            MorphInfo = Program.MorphAnalyzer.Parse(new string[] {unit}).First();
            Lemma = MorphInfo.BestTag.Lemma;
            this.Text = unit;
        }
    }
}
