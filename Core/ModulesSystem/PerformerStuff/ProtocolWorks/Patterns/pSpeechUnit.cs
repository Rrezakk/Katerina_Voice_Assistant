using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows.Forms;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using static K3NA_Remastered_2.Program;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class PSpeechUnit
    {
        public string VariableName;
        public string TypeString;
        public bool IsVariable = false;
        public List<MorphInfo> Morph;//https://github.com/lepeap/DeepMorphy/blob/master/gram.md
        public PSpeechUnit(){}
        public PSpeechUnit(string fillery)
        {
            Fill(fillery);
        }
        //public List<string> MorphToStrings() => Morph.Select(m => m.BestTag.ToString()).ToList();
        //public List<string> TextStrings() => Morph.Select(m => m.Text).ToList();
        //public List<string> LemmasStrings() => Morph.Select(m => m.BestTag.Lemma).ToList();
        public void Fill(string unit)
        {
            //<word:any:привет|здорова|хеллоу>
            //<var:type:name>
            //<word:morph:concretemorph>
            //<word:similar:каланхоэ>
            //слово
            if (unit.Contains("<"))
            {
                ProtocolsParser.ParseTripleUnit(unit, out var chema, out var type,
                    out var text);
                TypeString = type;
                switch (chema)
                {
                    case "var":
                    {
                        IsVariable = true;
                            //сопоставление типа и морфологии
                            //type -> MorphInfo 
                            //simmilar as word with type morph
                            //morph processor
                            VariableName = text;
                        Morph = null;//убрать заглушку
                        break;
                    }
                    case "word":
                    {
                        switch (TypeString)
                        {
                            case "any":
                            {
                                var args = text.Split("|");
                                Morph = MorphAnalyzer.Parse(args).ToList() /*.First()*/;
                                break;
                            }
                            case "similar":
                            {
                                Morph = MorphAnalyzer.Parse(text).ToList() /*.First()*/;
                                break;
                            }
                            case "morph":
                            {
                                //morph processor
                                //List<MorphInfo> Morph with 1 unit
                                break;
                            }
                            case "anymorph":
                            {
                                var args = text.Split("|");
                                foreach (var morphString in args)
                                {
                                    //morph processor for morphString
                                }
                                //List<MorphInfo> Morph with args.count elements
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Morph = Program.MorphAnalyzer.Parse(new []{unit}).ToList();
                TypeString = "common";
                //single word
            }
        }
    }
}
