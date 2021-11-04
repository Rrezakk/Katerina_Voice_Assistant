using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;

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
            //throw new NotImplementedException();
            return new List<Variable>();
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
            var variables = ExtractVariables(protocol.GetPattern(), speechPattern);//мы получили переменные
            foreach (var var in variables)
            {
                this.SetVariable(var);//глупо, но с закосом на будущее
            }
            var commands = protocol.GetCommands();
            this.FillArguments(ref commands);//вставляем переменные в аргументы
            var container = new CommandsContainer(commands,this,protocol.Name);//связываем
            Core.CommandsExecutor.EnqueueNew(container); //выставляем команды в очередь на выполнение
        }
    }
}
