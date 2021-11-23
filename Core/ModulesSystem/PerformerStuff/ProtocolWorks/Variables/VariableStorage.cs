using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public void SetVariable(string name, string content, string filler = "unknown")
        {
            foreach (var t in _variables.Where(t => t.Name == name))
            {
                t.Content = content;
                t.LastFilledBy = filler;
                return;
            }
            _variables.Add(new Variable(name,content,filler));
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"Variable storage:\n");
            foreach (var variable in this._variables)
            {
                sb.Append($"Var: \'{variable.Name}\' -> \'{variable.Content}\'\n");
            }
            sb.Append($"-----------------");
            return sb.ToString();
        }
    }
}
