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
        public SpeechUnit()
        {
            this.Type = Types.Word.Empty;
        }
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
        public SpeechUnit(string varName, Types.Word vartype)
        {
            this.VariableName = varName;
            this.Type = vartype;// switch
            //{
            //    "singleWord" => Types.Word.VarSingleWord,
            //    "multipleWord" => Types.Word.VarMultipleWord,
            //    _ => this.Type
            //};
            
        }
        public void AboutMe()
        {
            Console.WriteLine($"Type: {Type}");
            Console.WriteLine($"Data: {Data}");
            Console.WriteLine($"DataStrings: {DataStrings}");
            Console.WriteLine($"VariableName: {VariableName}");
        }
    }
}
