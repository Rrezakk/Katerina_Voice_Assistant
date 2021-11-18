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
        public string Raw = "";
        public string VariableName;
        public string TypeString;
        public bool IsVariable = false;
        private List<MorphInfo> _morph;
        public List<MorphInfo> Morph
        {
            get => _morph;
            private set => _morph = value;
        } //https://github.com/lepeap/DeepMorphy/blob/master/gram.md
        public PSpeechUnit(){}
        public PSpeechUnit(string fillery)
        {
            Fill(fillery);
        }
        public override string ToString()
        {
            return $"[type: {TypeString} Raw: {Raw}]";
        }
        public void Fill(string unit)
        {
            //<var:type:name>
            //<var:anysimilar.завтра|сегодня|вчера|когда-нибудь:varname>
            //<var:similar.имя:varname>
            //<var:anysimilar.#LISTNAME_WList:varname>

            ////<word:any:привет|здорова|хеллоу>
            //<word:morph:concretemorph>
            //<word:similar:каланхоэ>
            //<word:anysimilar:завтра|сегодня|вчера|когда-нибудь>
            //слово
            this.Raw = unit;
            if (unit.Contains("<"))
            {
                ProtocolsParser.ParseTripleUnit(unit, out var chema, out var type,
                    out var text);
                var container = "";//это аргументы для типов вроде anysimilar
                TypeString = type;
                if (type.Contains('.'))
                {
                    var dotIndex = type.IndexOf('.');
                    TypeString = type.Substring(0, dotIndex);
                    container = type.Substring(dotIndex + 1, type.Length - dotIndex - 1);
                }
                switch (chema)
                {
                    case "var":
                    {
                        IsVariable = true;
                        switch (TypeString)
                        {
                                case "similar":
                                    Morph = MorphAnalyzer.Parse(container).ToList();
                                break;
                                case "anysimilar":
                                    Morph = AnySimilarProcessor(container);//в будущем добавить поддержку многословных эталонов
                                    break;
                                case "newType?Fuck!How to create it?!":
                                    break;
                        }
                        VariableName = text;
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
                throw new Exception("Under developement: [anysimilar.#NAME_WList] model");
            }
            else
            {
                var array = text.Split('|');
                return MorphAnalyzer.Parse(array).ToList();
            }
        }
    }
}
