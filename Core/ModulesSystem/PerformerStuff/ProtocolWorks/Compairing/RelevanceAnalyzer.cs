using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Compairing
{
    public class RelevanceAnalyzer
    {
        public static double GetRelevance(sSpeechPattern speechPattern, PSpeechPattern protocolPattern)//по сути мы перебираем все протоколы в цикле, нам легко получить протокол затем по индексу в таблице
        {
            if (protocolPattern.Units.Count==0)
            {
                return 0d;
            }
            var countRel = ((double)protocolPattern.Units.Count -
                            Math.Abs((double)protocolPattern.Units.Count - (double)speechPattern.Units.Count)) /
                (double)protocolPattern.Units.Count*100d;
            Console.WriteLine($"Units processing:");
            var protoUnitsCount = protocolPattern.Units.Count;
            var commulative = 0d;
            if (protoUnitsCount>speechPattern.Units.Count)
            {
                protoUnitsCount = speechPattern.Units.Count;
            }

            for (var i = 0; i < protoUnitsCount; i++)
            {
                //unit1 = protocolPattern.Units[i]
                //unit2 = speechPattern.Units[i]
                if (protocolPattern.Units[i].IsVariable)
                {
                    Console.WriteLine($"Variable: 0");
                    continue;
                }
                var compareResult = 0d;
                switch (protocolPattern.Units[i].TypeString)
                {
                    case "common":
                        compareResult = CommonUnitsCompare(speechPattern.Units[i].MorphInfo, protocolPattern.Units[i].Morph.First());
                        commulative += compareResult;
                        Console.WriteLine($"Common: {compareResult}");
                        break;
                    case "similar":
                        compareResult = SimilarUnitsCompare(speechPattern.Units[i].MorphInfo, protocolPattern.Units[i].Morph.First());
                        commulative += compareResult;
                        Console.WriteLine($"Similar: {compareResult}");
                        break;
                    case "any":
                        compareResult = AnyUnitsCompare(speechPattern.Units[i].MorphInfo, protocolPattern.Units[i].Morph);
                        commulative += compareResult;
                        Console.WriteLine($"Any: {compareResult}");
                        break;
                }
            }
            Console.WriteLine($"CountRel: {countRel}");
            return (commulative + countRel) / (protocolPattern.Units.Count+1)/*processedUnits*/;

        }

        private static double AnyUnitsCompare(MorphInfo speech, IEnumerable<MorphInfo> protos)
        {
            var table = protos.Select(proto => CommonUnitsCompare(speech, proto)).ToList();
            return table.Max();
        }
        private static double CommonUnitsCompare(MorphInfo speech, MorphInfo proto,double priority = 0.8d)
        {
            var st = CutBestTag(speech.BestTag).Split(',');
            var pt = CutBestTag(proto.BestTag).Split(',');
            var total = pt.Length;
            var positive = pt.Count(tag => st.Contains(tag));
            var first = ((double)positive / (double)total)*100d;
            var cuttedFirst = Currentword(proto.Text);
            var cuttedSecond = Currentword(speech.Text);
            var ifConverive = (cuttedFirst.Contains(cuttedSecond)) ||(cuttedSecond.Contains(cuttedFirst));
            var second = ifConverive ? 100d : 0d;
            return second * priority + (1- priority) * first;
        }
        private static double SimilarUnitsCompare(MorphInfo speech, MorphInfo proto)
        {
            var st = CutBestTag(speech.BestTag).Split(',');
            var pt = CutBestTag(proto.BestTag).Split(',');
            var acc = pt.Where((t1, i) => st.Any(t => t1 == st[i])).Count();
            //Console.WriteLine($"{speech.BestTag} {proto.BestTag} {acc} {pt.Length} {(double)acc /pt.Length*100d}");
            return ((double)acc / (double)pt.Length) * 100d;
        }
        private static string Currentword(string currentword)
        {
            int curl;
            if (currentword.Length <= 7 & currentword.Length > 4) { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.75, 0)); }
            else if (currentword.Length <= 4) { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.85, 0)); }
            else { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.67, 0)); }
            return currentword.Substring(0, curl);
        }

        private static string CutBestTag(Tag tag)
        {
            var index = tag.ToString().IndexOf("Tags:", StringComparison.Ordinal) + "Tags:".Length;
            return tag.ToString().Substring(index, tag.ToString().Length - index).Trim(' ');
        }
        private static double Lemmatic(string[] speechLemma, string[] patternLemma)
        {
            return 0d;
        }
        private static double MorphologicalLinnear(MorphInfo speechMorphInfo,MorphInfo patternMorphInfo)
        {
            var speechTags = speechMorphInfo.BestTag.ToString().Split(' ');/////////////////////
            var patternTags = patternMorphInfo.BestTag.ToString().Split(' ');//////////////////////
            var accumulator = patternTags.Count(t => speechTags.Any(t1 => t == t1));
            var ans = accumulator / patternTags.Length * 100;
            Console.WriteLine($"Morphological: {accumulator}/{patternTags.Length} : {ans}");
            return ans;
        }

    }
}
