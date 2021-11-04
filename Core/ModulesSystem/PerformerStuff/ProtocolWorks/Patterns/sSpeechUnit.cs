using System.Linq;
using DeepMorphy.Model;

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
            this.MorphInfo = Core.MorphAnalyzer.Parse(new string[] {unit}).First();
            this.Lemma = MorphInfo.BestTag.Lemma;
            this.Text = unit;
        }
    }
}
