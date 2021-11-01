using System.Linq;
using DeepMorphy.Model;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns
{
    public class sSpeechUnit
    {
        public readonly MorphInfo MorphInfo;
        public string Text;
        public readonly string Lemma;
        public sSpeechUnit()
        {
            this.MorphInfo = null;
            this.Text = "";
            this.Lemma = "";
        }
        public sSpeechUnit(string unit)
        {
            this.MorphInfo = Program.MorphAnalyzer.Parse(new string[] {unit}).First();
            this.Lemma = MorphInfo.BestTag.Lemma;
            this.Text = unit;
        }
    }
}
