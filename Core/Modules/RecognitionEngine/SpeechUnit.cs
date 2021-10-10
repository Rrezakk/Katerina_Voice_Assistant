using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.RecognitionEngine
{
    public class SpeechUnit
    {
        public string Data;
        public string[] DataStrings;
        public Types.Word Type;
        public string VariableName;
        public readonly Types.Variable VariableType;
        public SpeechUnit(string data)
        {
            this.Data = data;
            this.Type = Types.Word.SingleWord;
        }
        public SpeechUnit(string[] data)
        {
            this.DataStrings = data;
            this.Type = Types.Word.MultipleWord;
        }
        public SpeechUnit(string varName,string vartype)
        {
            this.VariableName = varName;
            this.VariableType = vartype switch
            {
                "singleWord" => Types.Variable.SingleWord,
                "doubleWord" => Types.Variable.DoubleWord,
                _ => this.VariableType
            };
            this.Type = Types.Word.VariableWord;
        }
    }
}
