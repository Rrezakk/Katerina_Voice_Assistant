using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.Special;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public class VariableStorage//динамический класс, создается в контексте распознанного протокола, в дальнейшем полностью переделать
    {
        private class matrixElem
        {
            public matrixElem() { }

            public matrixElem(int line, int pos, double value)
            {
                this.line = line;
                this.pos = pos;
                this.value = value;
            }
            public int line;
            public int pos;
            public double value;
        }
        public VariableStorage()
        {
            _variables = new List<Variable>();
        }
        public VariableStorage(List<Variable> initialVariables)
        {
            _variables = initialVariables;
        }
        private readonly List<Variable> _variables;
        public Variable TryGetVariable(string name)//maybe convert later to out bool form
        {
            try
            {
                var variable = _variables.First(v => v.Name == name);
                return variable ?? new Variable(name,"");
            }
            catch (Exception)
            {
                return new Variable(name, "");
            }
        }
        public Variable GetVariable(string name)
        {
            return _variables.First(v => v.Name == name);
        }
        public void SetVariable(Variable va)
        {
            foreach (var t in _variables.Where(t => t.Name == va.Name))
            {
                t.Content = va.Content;
                t.LastFilledBy = va.LastFilledBy;
                return;
            }
            _variables.Add(va);
        }
        public void SetVariable(string name, string content, string filler = "VariableStorage setter")
        {
            foreach (var t in _variables.Where(t => t.Name == name))
            {
                t.Content = content;
                t.LastFilledBy = filler;
                return;
            }
            _variables.Add(new Variable(name,content,filler));
        }
        private static List<Variable> ExtractVariables(PSpeechPattern protocolPattern, SSpeechPattern speechPattern)
        {
            Console.WriteLine("Extracting variables...");
            var variableArr = new List<int>();
            for (var i = 0; i < protocolPattern.Units.Count; i++)
            {
                if (protocolPattern.Units[i].IsVariable)
                    variableArr.Add(i);
            }

            if (variableArr.Count==0)
            {
                return new List<Variable>();
            }
            else if (variableArr.Count == 1)
            {
                //three variants: var on first index or on last or on random in the middle
                var index = variableArr[0];
                if (index == 0)//single variable and positon is 0 index
                {
                    var matrix = new List<List<matrixElem>>();//матрица перестановок (сортировки)
                    var exponentialDiagonalicMatrix = Matrix.CreateDiagonalic(Matrix.ExponentialRegression, protocolPattern.Units.Count, speechPattern.Units.Count);//матрица - сомножитель
                    var relevanceArray = new double[protocolPattern.Units.Count, speechPattern.Units.Count];//массив релевантности
                    for (var i = 0; i < protocolPattern.Units.Count; i++)
                    {
                        for (var j = 0; j < speechPattern.Units.Count; j++)
                        {
                            var relevanceElem =
                                RelevanceAnalyzer.SingleRelevance(protocolPattern, speechPattern, i, j);
                            relevanceArray[i, j] = relevanceElem;
                        }
                    }//заполняем массив релевантностью пар
                    var mtrx = new Matrix(relevanceArray);//создаем объект матрицы для удобной работы
                    Console.WriteLine(mtrx);//визуализация релевантности
                    Console.WriteLine(exponentialDiagonalicMatrix*100);//визуализация сомножителя
                    mtrx = mtrx * exponentialDiagonalicMatrix;//умножение
                    Console.WriteLine(mtrx);//визуализация обработанной релевантности

                    for (var i = 0; i < mtrx.RowsCount; i++)
                    {
                        var relTable = new List<matrixElem>();
                        for (var j = 0; j < mtrx.ColumnsCount; j++)
                        {
                            relTable.Add(new matrixElem(i, j, mtrx.InnerDoubles[i,j]));
                        }
                        matrix.Add(relTable.OrderBy(d => -d.value).ToList());
                    }
                    foreach (var line in matrix)
                    {
                        foreach (var elem in line)
                        {
                            Console.Write("{0,6:F1}",elem.value);
                        }
                        Console.WriteLine();
                    }
                    //var locked = new List<int>();//индексы уже подобранных элементов
                    //var map = new Dictionary<int, int>();//map for 
                    //foreach (var line in matrix)//lines: phrase
                    //{
                    //    foreach (var elem in line)//elems: protocol elems
                    //    {
                    //        var f = locked.Any(lelem => lelem == elem.pos);
                    //        if (f) continue;
                    //        map.Add(elem.line, elem.pos);
                    //        locked.Add(elem.pos);
                    //        break;
                    //    }
                    //}
                    //foreach (var elem in map)
                    //{
                    //    var matrixelem = mtrx.InnerDoubles[elem.Key,elem.Value];
                    //    Console.WriteLine($"L:{elem.Key} : P:{elem.Value} -> {matrixelem} ");
                    //}
                    return new List<Variable>();
                }
                else if (index == protocolPattern.Units.Count)//single variable and positon is last index
                {
                    return new List<Variable>();
                }
                else
                {
                    return new List<Variable>();
                }
            }
            else if (variableArr.Count == 2)
            {
                return new List<Variable>();
            }
            else
            {
                return new List<Variable>();//not supported
            }
            //работает по схеме от краев к центру
            //throw new NotImplementedException();
            
        }
        private void FillArguments(ref List<Command> commands)
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
                        var variable = TryGetVariable(varName);
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
        public void ExecuteRecognizedProtocol(Protocol protocol, SSpeechPattern speechPattern)
        {
            Console.WriteLine($"SpeechPattern: {speechPattern.ToString()}");
            Console.WriteLine($"ProtoPattern: {protocol.GetPattern().ToString()}");
            var variables = ExtractVariables(protocol.GetPattern(), speechPattern);//мы получили переменные
            foreach (var var in variables)
            {
                this.SetVariable(var);//глупо, но с закосом на будущее
            }
            var commands = protocol.GetCommands();
            this.FillArguments(ref commands/*,this._variables*/);//вставляем переменные в аргументы
            var container = new CommandsContainer(commands,this,protocol.Name);//связываем
            Core.SubModules.CommandsExecutor().EnqueueNew(container); //выставляем команды в очередь на выполнение
        }
    }
}
