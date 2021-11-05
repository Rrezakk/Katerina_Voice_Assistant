using System;
using System.Collections.Generic;
using System.Linq;
using DeepMorphy.Model;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Tables;
using static K3NA_Remastered_2.Core;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns
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
            //<var:anysimilar:завтра|сегодня|вчера|когда-нибудь>
            //<word:morph:concretemorph>
            //<word:similar:каланхоэ>
            //<word:anysimilar:завтра|сегодня|вчера|когда-нибудь>
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
                        switch (TypeString)
                        {
                                case "anysimilar":
                                    Morph = AnySimilarProcessor(text);//в будущем добавить поддержку многословных эталонов
                                    break;
                                case "newType?Fuck!How to create it?!":
                                    break;
                        }
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
                            case "anysimilar":
                            {
                                Morph = AnySimilarProcessor(text);
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else//common single word
            {
                Morph = Core.MorphAnalyzer.Parse(new []{unit}).ToList();
                TypeString = "common";
            }
        }
        private static List<MorphInfo> AnySimilarProcessor(string text)
        {
            if (text.StartsWith("#") && text.EndsWith("_WList"))
            {
                throw new Exception("Under developement: [anysimilar: #NAME_WList] model");
            }
            else
            {
                var array = text.Split('|');
                return MorphAnalyzer.Parse(array).ToList();
            }
        }
    }
}
