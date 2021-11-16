using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Compairing;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public class VariableStorage//динамический класс, создается в контексте распознанного протокола, в дальнейшем полностью переделать
    {
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
                    const double minRel = 49d;
                    var markupArray = new double[protocolPattern.Units.Count];
                    var acc = 0d;
                    var maxRel = 0d;
                    var maxIndex = -1;
                    for (var i = protocolPattern.Units.Count - 1; i >= 0; i--)
                    {
                        var relevance = RelevanceAnalyzer.SingleRelevance(speechPattern, protocolPattern, i, i);
                        var positiveIndex = i - protocolPattern.Units.Count + 1;
                        markupArray[positiveIndex] = relevance;
                        acc += relevance;
                        if (acc/ positiveIndex > maxRel)
                        {
                            maxRel = acc / positiveIndex;
                            maxIndex = positiveIndex;
                        }

                    }
                    //var protoUnits = protocolPattern.Units;
                    //var removedCount = 0;
                    //for (int i = protoUnits.Count - 1; i >= 0; i--)
                    //{
                    //    var protoUnit = protoUnits[i];
                    //    var relevanceList = new List<double>();
                    //    for (int j = speechPattern.Units.Count - 1; j >= 0; j--)
                    //    {
                    //        var relevance = RelevanceAnalyzer.SingleRelevance(speechPattern, protocolPattern, j, i-removedCount);
                    //        relevanceList.Add(relevance);
                    //    }
                    //    var maxrel = 0d;
                    //    var maxRelIndex = 0;
                    //    for (int j = 0; j < relevanceList.Count; j++)
                    //    {
                    //        if (relevanceList[j] > maxrel)
                    //        {
                    //            maxrel = relevanceList[j];
                    //            maxRelIndex = j;
                    //        }
                    //    }

                    //}
                }
                else if (index == protocolPattern.Units.Count)//single variable and positon is last index
                {

                }
                else
                {

                }
            }
            else if (variableArr.Count == 2)
            {

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
