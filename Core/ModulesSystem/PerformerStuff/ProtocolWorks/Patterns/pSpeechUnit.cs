using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using static K3NA_Remastered_2.Program;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class pSpeechUnit
    {
        public string VariableName;
        public string VariableTypeString;
        public bool IsVariable = false;
        public MorphInfo Morph;//https://github.com/lepeap/DeepMorphy/blob/master/gram.md
        public pSpeechUnit(){}
        public pSpeechUnit(string fillery)
        {
            Fill(fillery);
        }
        public void Fill(string unit)
        {
            //<word:any:привет|здорова|хеллоу>
            //<var:type:name>
            //слово
            if (unit.Contains("<"))
            {
                ProtocolsParser.ParseTripleUnit(unit, out var chema, out var type,
                    out var text);
                switch (chema)
                {
                    case "var":
                    {
                        IsVariable = true;
                        //сопоставление типа и морфологии
                        //type -> MorphInfo 
                        VariableName = text;
                        VariableTypeString = type;
                        Morph = null;//убрать заглушку
                        break;
                    }
                    case "word":
                    {
                        var args = text.Split("|");
                        Morph = Program.MorphAnalyzer.Parse(args).First();
                        break;
                    }
                }
            }
            else
            {
                Morph = Program.MorphAnalyzer.Parse(new []{unit}).First();
                //single word
            }
        }

        
    }
}
