using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using static K3NA_Remastered_2.Program;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class pSpeechUnit
    {
        public string VariableName;
        public IEnumerable<MorphInfo> MorphList;//https://github.com/lepeap/DeepMorphy/blob/master/gram.md
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
                ProtocolsParser.ParsePatternUnit(unit, out var chema, out var type,
                    out var text);
                switch (chema)
                {
                    case "var":
                        //сопоставление типа и морфологии
                        //type -> MorphInfo 
                        VariableName = text;
                        break;
                    case "word":
                    {
                        var args = text.Split("|");
                        MorphList = Program.MorphAnalyzer.Parse(args);
                        break;
                    }
                }
            }
            else
            {
                MorphList = Program.MorphAnalyzer.Parse(new []{unit});
                //single word
            }
            

        }

        //public enum UnitType
        //{
        //    Word,
        //    Var
        //}
        //public enum CountType
        //{
        //    Single,
        //    Multiple
        //}

        //public UnitType GetType(string type)
        //{
        //    Enum.TryParse(type, out Types tp);
        //    return tp;
        //}
        //public UnitType unitType;
        //public CountType countType;
        //public IEnumerable<MorphInfo> MorphList;


        //public void Fill(string unitString, string type)
        //{
        //    this.unitType = GetType(type);
        //    if (this.unitType == UnitType.Word)
        //    {
        //        if (countType==CountType.Single)
        //        {
        //            MorphList = MorphAnalyzer.Parse(new string[] {unitString});

        //        }
        //        else if (countType==CountType.Multiple)
        //        {

        //        }
        //        //MorphAnalyzer
        //    }
        //    else if (this.unitType == UnitType.Var)
        //    {
        //        var varName = ProtocolsParser.
        //    }
        //    else
        //    {
        //        throw new ArgumentOutOfRangeException($"Filling pSpeechUnit error  ---  wrong type: {type}, parsed to: {this.unitType}");
        //    }
        //}
    }
}
