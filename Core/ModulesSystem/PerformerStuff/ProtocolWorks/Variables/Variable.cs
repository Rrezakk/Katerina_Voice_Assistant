using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Variables
{
    public class Variable
    {
        public readonly string Name;
        public string Content;
        public readonly int Id = VariableIdGenerator.NextId();
        public string LastFilledBy;//эту механику можно использовать как контекст
        public Variable()
        {
            Name = "";
            Content = "";
            LastFilledBy = "constructor";
        }
        public Variable(string name, string content, string filler = "constructor")
        {
            Name = name;
            Content = content;
            LastFilledBy = filler;
        }
    }

    public static class VariableIdGenerator
    {
        private static int _id = 0;
        public static int NextId() => _id++;
    }
}
