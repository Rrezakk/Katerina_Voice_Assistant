﻿//#define Print_Percentage//used for enabling console output for debugging

using System;
using System.Collections.Generic;
using System.Linq;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special;
using TensorFlow;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing
{
    public static class RelevanceAnalyzer
    {
        private const double MinRelevance = 15f;

        public static Protocol GetRelevantProtocol(SSpeechPattern speechPattern, List<Protocol> protocols)
        {
            foreach (var protocol in protocols)
            {
                var matrix = new List<List<MatrixElem>>();//матрица перестановок (сортировки)
                Console.WriteLine($"P: {protocol.GetPattern()}");
                var protocolPattern = protocol.GetPattern();
                if (protocolPattern.Units.Count == 0 || speechPattern.Units.Count == 0)
                    continue;
                var exponentialDiagonalicMatrix = Matrix.CreateDiagonalic(Matrix.ExponentialRegression, protocolPattern.Units.Count, speechPattern.Units.Count);//матрица - сомножитель
                var relevanceArray = new float[protocolPattern.Units.Count, speechPattern.Units.Count];//массив релевантности
                for (var i = 0; i < protocolPattern.Units.Count; i++)
                {
                    for (var j = 0; j < speechPattern.Units.Count; j++)
                    {
                        var relevanceElem = (float)Math.Round(SingleRelevance(protocolPattern, speechPattern, i, j), 2);
                        relevanceArray[i, j] = relevanceElem;
                    }
                }//заполняем массив релевантностью пар
                var mtrx = new Matrix(relevanceArray);//создаем объект матрицы для удобной работы
                //Console.WriteLine(mtrx);//визуализация релевантности
                //Console.WriteLine(exponentialDiagonalicMatrix * 100);//визуализация сомножителя
                mtrx *= exponentialDiagonalicMatrix;//умножение
                Console.WriteLine(mtrx);//визуализация обработанной релевантности



                for (var i = 0; i < mtrx.RowsCount; i++)
                {
                    var relTable = new List<MatrixElem>();//phrase parts to protocol part
                    for (var j = 0; j < mtrx.ColumnsCount; j++)
                    {
                        relTable.Add(new MatrixElem(i, j, mtrx.Innerfloats[i, j]));
                    }
                    matrix.Add(relTable.OrderBy(d => -d.Value).ToList());
                }//заполнение матрицы сортировки
                foreach (var line in matrix)
                {
                    foreach (var elem in line)
                    {
                        Console.Write("{0,6:F1}", elem.Value);
                    }
                    Console.WriteLine();
                }//вывод матрицы сортировки
                var minRelevance = Math.Round(2.5f * Matrix.GetMinForExponentialDiagonalic(exponentialDiagonalicMatrix), 2);//минимальная релевантность
                Console.WriteLine($"Min relevance: {minRelevance}");

                var list = new List<int>(); //for indexes in lines to calculate %
                var acc = 0f;
                foreach (var relTable in matrix)
                {
                    foreach (var t in relTable.Where(t => !list.Contains(t.Col)))
                    {
                        if ((t.Value < minRelevance)) continue;
                        
                        if (!list.Contains(t.Col))
                        {
                            list.Add(t.Col);
                            acc+=t.Value;
                        }
                        break;
                    }
                }

                var metrica = (acc / list.Count) * (1f / (matrix[0].Count - list.Count + 1));
                Console.WriteLine(string.Join(' ',list) +"  - " + metrica);
            }

            throw new NotImplementedException();
        }

        public static Protocol GetMaxRelevanceProtocol(SSpeechPattern phrase,List<Protocol> protocols)
        {
            var relTable = protocols.Select(protocol => MultipleRelevance(phrase, protocol.GetPattern())).ToList();
            var outstr = "";
            var max = 0f;
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
        private static float MultipleRelevance(SSpeechPattern speechPattern, PSpeechPattern protocolPattern)//по сути мы перебираем все протоколы в цикле, нам легко получить протокол затем по индексу в таблице
        {
            if (protocolPattern.Units.Count==0)
            {
                return 0f;
            }
            var countRel = (protocolPattern.Units.Count -
                            Math.Abs(protocolPattern.Units.Count - (float)speechPattern.Units.Count)) /
                protocolPattern.Units.Count*100f;//релевантность совпадения по количеству элементов
            var protoUnitsCount = protocolPattern.Units.Count;
            var commulative = 0f;
            if (protoUnitsCount >speechPattern.Units.Count)//ограничение количества итераций по количеству юнитов протокола
            {
                protoUnitsCount = speechPattern.Units.Count;
            }

            Console.WriteLine("Units processing:");
            Console.WriteLine($"CountRel: {countRel}");
            for (var i = 0; i < protoUnitsCount; i++)
            {
                commulative += SingleRelevance(protocolPattern,speechPattern,i,i);
            }
            return (commulative + countRel) / (protocolPattern.Units.Count+1f)/*processedUnits*/;
        }
        public static float SingleRelevance(PSpeechPattern protocolPattern, SSpeechPattern speechPattern, int i,int j)
        {
            var compareResult = 0f;//result
            var speechUnit = speechPattern.Units[j];
            var protoUnit = protocolPattern.Units[i];
            PrintRelevance($"Singlerelevance: {protoUnit.Raw} {speechUnit.Text}");
            if (protoUnit.IsVariable)
            {
                switch (protoUnit.TypeString)
                {
                    case "anysimilar":
                        compareResult = AnySimilarUnitsCompare(protocolPattern, speechPattern, i,j);
                        PrintRelevance($"AnySimilar (VARIABLE): {compareResult}"); /*for: {protoUnit.Raw} {speechUnit.Text}*/
                        break;
                    case "singleWord":
                        compareResult = SingleWordCompare(protocolPattern, speechPattern, i,j);
                        PrintRelevance($"SingleWord (VARIABLE): {compareResult}");
                        break;
                    default:
                        compareResult = 0f;
                        PrintRelevance($"Unknown protoUnit type: {protoUnit.Raw} -> 0 relevance");
                        break;
                }
                return compareResult;//exit
            }
            //if isVariable -> doesn't continue
            switch (protoUnit.TypeString)
            {
                case "common":
                    compareResult = CommonUnitsCompare(speechUnit.MorphInfo, protoUnit.Morph.First());
                    PrintRelevance($"Common: {compareResult}");
                    break;
                case "similar":
                    compareResult = SimilarUnitsCompare(speechUnit.MorphInfo, protoUnit.Morph.First());
                    PrintRelevance($"Similar: {compareResult}");
                    break;
                case "any":
                    compareResult = AnyUnitsCompare(speechUnit.MorphInfo, protoUnit.Morph);
                    PrintRelevance($"Any: {compareResult}");
                    break;
                case "morph":
                    compareResult = 0f;
                    PrintRelevance($"morph is not supported yet: {compareResult}");
                    break;
                case "anymorph":
                    compareResult = 0f;
                    PrintRelevance($"Anymorph is not supported yet: {compareResult}");
                    break;
                case "anysimilar":
                    compareResult = AnySimilarUnitsCompare(protocolPattern, speechPattern, i,j);
                    PrintRelevance($"AnySimilar: {compareResult}");
                    break;
                default:
                    compareResult = 0f;
                    PrintRelevance($"Unknown protoUnit type: {protoUnit.Raw} -> 0 relevance");
                    break;
            }
            return compareResult;
        }
        private static void PrintRelevance(string text)
        {
#if Print_Percentage
            Console.WriteLine(text);
#endif
        }
        private static float SingleWordCompare(PSpeechPattern proto,SSpeechPattern speech,int i,int j)
        {
            var len = speech.Units[j].MorphInfo.Text.Length;
            return len switch
            {
                <= 3 => 1f,
                <= 6 => 2f + (len - 3f) / 4f,
                _ => 1f
            };
        }
        private static float AnySimilarUnitsCompare(PSpeechPattern protocolPattern,SSpeechPattern speechPattern,int i,int j)
        {
            var u = new float[protocolPattern.Units[i].Morph.Count];
            var max = 0d;
            var maxIndex = 0;
            for (var index = 0; index < u.Length; index++)
            {
                u[index] = SimilarUnitsCompare(speechPattern.Units[j].MorphInfo,
                    protocolPattern.Units[i].Morph[index]);
                if (!(u[index] > max)) continue;
                max = u[index];
                maxIndex = index;
            }
            return  u[maxIndex];
        }
        private static float AnyUnitsCompare(MorphInfo speech, IEnumerable<MorphInfo> protos)
        {
            var table = protos.Select(proto => CommonUnitsCompare(speech, proto)).ToList();
            return table.Max();
        }
        private static float CommonUnitsCompare(MorphInfo speech, MorphInfo proto,float priority = 0.8f)
        {
            var st = CutBestTag(speech.BestTag).Split(',');
            var pt = CutBestTag(proto.BestTag).Split(',');
            var total = pt.Length;
            var positive = pt.Count(tag => st.Contains(tag));
            var first = (positive / (float)total)*100f;
            var cuttedFirst = Currentword(proto.Text);
            var cuttedSecond = Currentword(speech.Text);
            var ifConverive = (cuttedFirst.Contains(cuttedSecond)) ||(cuttedSecond.Contains(cuttedFirst));
            var second = ifConverive ? 100f : 0f;
            return second * priority + (1f- priority) * first;
        }
        private static float SimilarUnitsCompare(MorphInfo speech, MorphInfo proto)
        {
            var st = CutBestTag(speech.BestTag).Split(',');
            var pt = CutBestTag(proto.BestTag).Split(',');
            var positive = st.Count(speechTag => pt.Any(protocolTag => speechTag == protocolTag));
            return (positive / (float)pt.Length) * 100f;
        }
        private static string Currentword(string currentword)
        {
            int curl;
            if (currentword.Length <= 7 & currentword.Length > 4) { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.75, 0)); }
            else if (currentword.Length <= 4) { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.85, 0)); }
            else { curl = Convert.ToInt16(Math.Round(currentword.Length * 0.67, 0)); }
            return currentword[..curl];
        }
        private static string CutBestTag(Tag tag)
        {
            var index = tag.ToString().IndexOf("Tags:", StringComparison.Ordinal) + "Tags:".Length;
            var result = tag.ToString().Substring(index, tag.ToString().Length - index).Trim(' ');
            return result ?? "";
        }
    }
}
