using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special;
using K3NA_Remastered_2.LanguageExtensions;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public static class VariablesProcessor
    {
        public static List<Variable> ExtractVariables(PSpeechPattern protocolPattern, SSpeechPattern speechPattern)
        {
            if (speechPattern.Units.Count == 0 || protocolPattern.Units.Count == 0) return new List<Variable>();
            var variables = new List<Variable>();
            Console.WriteLine("Extracting variables...");


            var matrix = RelevanceAnalyzer.GetRelevanceMatrix(speechPattern, protocolPattern);
            var orderingMatrix = Matrix.CreateDiagonalic(Matrix.DefaultRegression/*Matrix.ExponentialRegression*/,
                protocolPattern.Units.Count, speechPattern.Units.Count);//матрица - сомножитель
            matrix *= orderingMatrix;
            var permutationMatrix = RelevanceAnalyzer.GetPermutationMatrix(matrix);

            var minRelevance = Math.Round(2.5f * Matrix.GetMinForDegressiveDiagonalic(orderingMatrix), 2);//минимальная релевантность
            Console.WriteLine($"Min relevance: {minRelevance}");
            var map = new Dictionary<int, int>();//pattern unit -> speech unit map
            var errorMap = new Dictionary<int, string>();//pattern unit -> empty string                                                  later will be       ####int[]
            for (var i = 0; i < permutationMatrix.Count; i++)
            {
                var lineElements = permutationMatrix[i]; //find max in line, excluding low relevant and already used
                foreach (var t in lineElements.Where(t => !map.ContainsKey(t.Col)))
                {
                    if (/*!*/(t.Value < minRelevance)) continue;//so important!!!!!
                    if (!errorMap.ContainsKey(i))
                    {
                        errorMap.Add(i, "");

                    }
                    else 
                    {
                        if (!map.ContainsKey(i))
                        {
                            map.Add(i, t.Col);
                        }
                        break;
                    }
                }
            }//mapping
            foreach (var (key, value) in map)
            {
                Console.WriteLine($"{key} -> {value}");
            }


            foreach (var (key, value) in map)
            {
                Console.WriteLine($"{protocolPattern.Units[key]} -> \"{speechPattern.Units[value]}\"");
                if (protocolPattern.Units[key].IsVariable)
                {
                    variables.Add(new Variable(protocolPattern.Units[key].VariableName, speechPattern.Units[value].Text));
                }
                
            }
            foreach (var (key, value) in errorMap)
            {
                Console.WriteLine($"{protocolPattern.Units[key]} -> \"{value}\"");
                if (protocolPattern.Units[key].IsVariable)
                {
                    variables.Add(new Variable(protocolPattern.Units[key].VariableName, value));
                }
            }
            return variables;
        }
        public static void FillArguments(VariableStorage storage,ref List<Command> commands)
        {
            Console.WriteLine($"Before filling arguments:");
            foreach (var command in commands)
            {
                Command.About(command);
            }
            foreach (var command in commands)
            {
                for (var k = 0; k < command.Arguments.Length; k++)
                {
                    var argument = command.Arguments[k];
                    if (argument.Contains("<"))
                    {
                        var i = argument.IndexOf("<", StringComparison.Ordinal) + "<".Length;
                        var j = argument.IndexOf(":", StringComparison.Ordinal);
                        string varName;
                        var vartype = "fill";
                        if (j != -1)//переменная с параметром вывода
                        {
                            varName = argument.Substring(i, j - i);
                            vartype = argument.Substring(j + 1, argument.IndexOf(">", StringComparison.Ordinal) - j - 1);
                        }
                        else//переменная без параметра вывода
                        {
                            j = argument.IndexOf(">", StringComparison.Ordinal);
                            varName = argument.Substring(i, j - i);
                        }
                        if (vartype == "out") throw new NotImplementedException();//dummy//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        var variable = storage.TryGetVariable(varName);
                        command.Arguments[k] = variable.Content;//error
                    }
                    else
                    {
                        command.Arguments[k] = argument;
                    }
                }
            }

            var horizontalCharactersLine = "-".Repeat(18);
            Console.WriteLine(horizontalCharactersLine);
            Console.WriteLine($"After filling arguments:");
            foreach (var command in commands)
            {
                Command.About(command);
            }
            Console.WriteLine(horizontalCharactersLine);
        }
    }
}
