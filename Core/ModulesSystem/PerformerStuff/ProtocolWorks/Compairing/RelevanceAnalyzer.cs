﻿using System;
using System.Collections.Generic;
using System.Linq;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing
{
    public static class RelevanceAnalyzer
    {
        public const double MinRelevance = 49d;
        public static Protocol GetMaxRelevanceProtocol(string phrase, List<Protocol> protocols)
        {
            return GetMaxRelevanceProtocol(new SSpeechPattern(phrase), protocols);
        }
        public static Protocol GetMaxRelevanceProtocol(SSpeechPattern phrase,List<Protocol> protocols)
        {
            var relTable = protocols.Select(protocol => RelevanceAnalyzer.GetRelevance(phrase, protocol.GetPattern())).ToList();
            var outstr = "";
            var max = 0d;
            foreach (var line in relTable)
            {
                if (line>max)
                {
                    max = line;
                }
                outstr += $"[{line:F1}],";
            }
            outstr = outstr.TrimEnd(',');
            var index = relTable.IndexOf(max);
            Console.WriteLine($"Relevance Table: {outstr}");
            Console.WriteLine($"Max: {max} on {index} : {protocols[index].Name}");
            return max< MinRelevance ? new UnknownProtocolType() : protocols[index];//узнать потом по имени протокола, нашелся ли подходящий
        }
        public static double GetRelevance(SSpeechPattern speechPattern, PSpeechPattern protocolPattern)//по сути мы перебираем все протоколы в цикле, нам легко получить протокол затем по индексу в таблице
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
                var compareResult = 0d;
                //unit1 = protocolPattern.Units[i]
                //unit2 = speechPattern.Units[i]
                if (protocolPattern.Units[i].IsVariable)
                {
                    switch (protocolPattern.Units[i].TypeString)
                    {
                        case "anysimilar":
                            compareResult = AnySimilarUnitsCompare(protocolPattern, speechPattern, i);
                            commulative += compareResult;
                            Console.WriteLine($"AnySimilar (VARIABLE): {compareResult}");
                            break;
                        default:
                            Console.WriteLine($"Variable: 0");
                            break;
                    }

                    continue;
                }
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
                    case "anysimilar":
                        compareResult = AnySimilarUnitsCompare(protocolPattern, speechPattern, i);
                        commulative += compareResult;
                        Console.WriteLine($"AnySimilar: {compareResult}");
                        break;
                }
            }
            Console.WriteLine($"CountRel: {countRel}");
            return (commulative + countRel) / (protocolPattern.Units.Count+1)/*processedUnits*/;
        }

        private static double AnySimilarUnitsCompare(PSpeechPattern protocolPattern,SSpeechPattern speechPattern,int i)
        {
            var u = new double[protocolPattern.Units[i].Morph.Count];
            var max = 0d;
            var maxIndex = 0;
            for (var j = 0; j < u.Length; j++)
            {
                u[j] = SimilarUnitsCompare(speechPattern.Units[i].MorphInfo,
                    protocolPattern.Units[i].Morph[j]);
                if (!(u[j] > max)) continue;
                max = u[j];
                maxIndex = j;
            }
            return  u[maxIndex];
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
