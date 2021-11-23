using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public static class VariablesProcessor
    {
        //move all the variables logic here
        //variable storage rename to variable context...maybe
        //bind protocol's patterns and storages
        public static List<Variable> ExtractVariables(PSpeechPattern protocolPattern, SSpeechPattern speechPattern)
        {
            var variables = new List<Variable>();
            Console.WriteLine("Extracting variables...");
            var matrix = new List<List<MatrixElem>>();//матрица перестановок (сортировки)
            var exponentialDiagonalicMatrix = Matrix.CreateDiagonalic(Matrix.ExponentialRegression, protocolPattern.Units.Count, speechPattern.Units.Count);//матрица - сомножитель
            var relevanceArray = new float[protocolPattern.Units.Count, speechPattern.Units.Count];//массив релевантности
            for (var i = 0; i < protocolPattern.Units.Count; i++)
            {
                for (var j = 0; j < speechPattern.Units.Count; j++)
                {
                    var relevanceElem = (float)Math.Round(RelevanceAnalyzer.SingleRelevance(protocolPattern, speechPattern, i, j), 2);
                    relevanceArray[i, j] = relevanceElem;
                }
            }//заполняем массив релевантностью пар
            var mtrx = new Matrix(relevanceArray);//создаем объект матрицы для удобной работы
            Console.WriteLine(mtrx);//визуализация релевантности
            Console.WriteLine(exponentialDiagonalicMatrix * 100);//визуализация сомножителя
            mtrx *= exponentialDiagonalicMatrix;//умножение
            Console.WriteLine(mtrx);//визуализация обработанной релевантности
            for (var i = 0; i < mtrx.RowsCount; i++)
            {
                var relTable = new List<MatrixElem>();
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
            var map = new Dictionary<int, int>();//pattern unit -> speech unit map
            var errorMap = new Dictionary<int, string>();//pattern unit -> empty string                                                  later will be       ####int[]
            for (var i = 0; i < matrix.Count; i++)
            {
                var lineElements = matrix[i]; //find max in line, excluding low relevant and already used
                foreach (var t in lineElements.Where(t => !map.ContainsKey(t.Pos)))
                {
                    if (t.Value < minRelevance)
                        errorMap.Add(i, "");
                    else
                    {
                        map.Add(i, t.Pos);
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
            Console.WriteLine($"After filling arguments:");
            foreach (var command in commands)
            {
                Command.About(command);
            }
        }
    }
}
