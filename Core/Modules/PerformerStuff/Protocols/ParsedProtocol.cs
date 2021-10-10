using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.Protocols
{
    public class ParsedProtocol
    {
        public string Name;
        public string Type;
        public string Commands;
        public string Pattern;
        public string Statement;
        public string StatementType;

        public ParsedProtocol(string name, string type, string commands, string pattern, string statement,string statementType)
        {
            this.Name = name;
            this.Type = type;
            this.Commands = commands;
            this.Pattern = pattern;
            this.Statement = statement;
            this.StatementType = statementType;
        }
    }
}
