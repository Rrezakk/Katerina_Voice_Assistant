using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Variables
{
    public class VariableStorage//динамический класс, создается в контексте распознанного протокола
    {
        private readonly List<Variable> Variables = new List<Variable>();

        public Variable GetVariable(string name)
        {
            return Variables.FirstOrDefault(v => v.Name == name);
        }
        public void SetVariable(string name, string content, string filler = "VariableStorage setter")
        {
            var variable = Variables.FirstOrDefault(v => v.Name == name);
            if (variable != null)
            {
                Variables[variable.Id].Content = content;
                Variables[variable.Id].LastFilledBy = filler;
            }
            else
            {
                Variables.Add(new Variable(name,content,filler));
            }
        }

    }
}
