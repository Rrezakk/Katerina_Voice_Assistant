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
        public static double GetRelevance(sSpeechPattern speechPattern, pSpeechPattern protocolPattern)//по сути мы перебираем все протоколы в цикле, нам легко получить протокол затем по индексу в таблице
        {
            speechPattern.GetRawLemma();//const on iterations
            protocolPattern.GetLemmaArray();
            //проходим алгоритмом по юнитам
            //переменные не трогаем
            return 0d;
        }

        private static double Lemmatic(string[] speechLemma, string[] patternLemma)
        {
            return 0d;
        }
        private static double MorphologicalLinnear(MorphInfo speechMorphInfo,MorphInfo patternMorphInfo)
        {
            var speechTags = speechMorphInfo.BestTag.ToString().Split(' ');
            var patternTags = patternMorphInfo.BestTag.ToString().Split(' ');
            var accumulator = patternTags.Count(t => speechTags.Any(t1 => t == t1));
            var ans = accumulator / patternTags.Length * 100;
            Console.WriteLine($"Morphological: {accumulator}/{patternTags.Length} : {ans}");
            return ans;
        }

    }
}
