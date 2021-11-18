using System;
using System.Linq;
using DeepMorphy.Model;
using NAudio.SoundFont;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns
{
    public class SSpeechUnit
    {
        public readonly MorphInfo MorphInfo;
        public string Text;
        public readonly string Lemma;
        public SSpeechUnit()
        {
            this.MorphInfo = null;
            this.Text = "";
            this.Lemma = "";
        }
        public SSpeechUnit(string unit)
        {
            this.MorphInfo = Core.MorphAnalyzer.Parse(new[] { unit }).First();
            this.Lemma = MorphInfo != null ? MorphInfo.BestTag.Lemma : "";
            this.Text = unit;
        }
        public override string ToString()
        {
            return $"{Text}";
        }
    }
}
