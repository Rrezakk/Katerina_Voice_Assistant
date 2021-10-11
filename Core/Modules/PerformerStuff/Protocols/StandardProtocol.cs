using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K3NA_Remastered_2.Modules.PerformerStuff.Protocols;
using K3NA_Remastered_2.Modules.RecognitionEngine;

namespace K3NA_Remastered_2.Modules.PerformerStuff.Protocols
{
    public class StandardProtocol
    {
        public string Name;
        public SpeechPattern Pattern;//just for now, later will be objects
        public string Commands;//just for now, later will be objects
        public StandardProtocol(){}
        public StandardProtocol(ParsedProtocol protocol)
        {
            this.Name = protocol.Name;
            this.Pattern = ParsePattern(protocol.Pattern);
            this.Commands = ParseCommands(protocol.Commands);
        }
        private static SpeechPattern ParsePattern(string pattern)
        {
            //parsing to speechUnits
            //pattern now is: <word:any:привет|здорова|хеллоу> слово <var:type:name>
            var units = StandardProtocolParser.GetUnits(pattern);
            foreach (var unit in units)
            {
                unit.AboutMe();
            }
            Console.WriteLine("-------------------------------------------");
            return new SpeechPattern();
        }
        private static string ParseCommands(string commands)
        {
            return commands;
        }
    }
    internal class StandardProtocolParser
    {
        public static List<SpeechUnit> GetUnits(string pattern)
        { 
            var patternArray = pattern.Split(' ');
            var patternList = patternArray.Select(t => t.Trim(' ')).Where(element => element != "").ToList();
            List<SpeechUnit> speechUnits = new List<SpeechUnit>();
            foreach (var unit in patternList)
            {
                SpeechUnit speechUnit = new SpeechUnit();
                var type = GetUnitType(unit);
                switch (type)
                {
                    case Types.Word.SingleWord: 
                        speechUnit = new SpeechUnit(unit);
                        break;
                    case Types.Word.MultipleWord:
                        var i = unit.IndexOf(":");
                        i = unit.IndexOf(":", i + 1);
                        var k = unit.IndexOf(">");
                        var multipleWordsString = unit.Substring(i, k - i);
                        var multipleWords = multipleWordsString.Split('|');
                        speechUnit = new SpeechUnit(multipleWords);
                        break;
                    case Types.Word.VarSingleWord:
                        var j = unit.IndexOf(":");
                        i = unit.IndexOf(":", j + 1);
                        var varType = unit.Substring(j, i - j);
                        var m = unit.IndexOf(">");
                        speechUnit = new SpeechUnit(unit.Substring(i+1, m - i-1), Types.Word.VarSingleWord);
                        break;
                    case Types.Word.VarMultipleWord:

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                speechUnits.Add(speechUnit);
            }

            return speechUnits;
            //var result = new SpeechUnit();
        }

        public static Types.Word GetUnitType(string unit)
        {
            if (unit.StartsWith("<word"))
            {
                return Types.Word.MultipleWord;
            }
            else if (unit.StartsWith("<var"))
            {
                //switch по vartype
                return Types.Word.VarSingleWord;
            }
            else
            {
                return Types.Word.SingleWord;
            }
        }
        

    }
}
