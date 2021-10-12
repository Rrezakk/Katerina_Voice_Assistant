using System;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns
{
    public class SpeechUnit
    {
        public enum WordType
        {
            SingleWord,
            Time,
            MultipleWord,
            Place,
            Number,
        }
        public enum UnitType
        {
            Variable,
            Word,
        }

        public WordType wordType;
        public UnitType unitType;

        private string data = "";
        private string[] dataStrings;
        private string varName = "";
        public SpeechUnit() { }

        public void AssertAsWord(object word)
        {
            this.unitType = UnitType.Word;
            this.wordType = Classificator.ClassifyWord(word);
            switch (wordType)
            {
                case WordType.SingleWord:
                    data = (string) word;
                    break;
                case WordType.Time:

                    break;
                case WordType.MultipleWord:
                    dataStrings = (string[])word;
                    break;
                case WordType.Place:

                    break;
                case WordType.Number:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        public void AssertAsVariable(object variable)
        {
            this.unitType = UnitType.Variable;
            data = (string) variable;
            wordType = Classificator.ClassifyVariable(variable);
            //data = Classificator.ClassifyVariable()
        }

        public void Get(out object data,out UnitType type)
        {
            switch (unitType)
            {
                case UnitType.Variable:
                    data = this.data;
                    type = UnitType.Variable;
                    break;
                case UnitType.Word:
                    data = this.data;
                    type = UnitType.Word;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
    public class Classificator
    {
        public static SpeechUnit.WordType ClassifyWord(object word)
        {
            string stringType = typeof(string).FullName;
            string stringArrayType = typeof(string[]).FullName;
            string wordType = word.GetType().FullName;

            if (wordType == stringType)
            {
                return SpeechUnit.WordType.SingleWord;
            }
            else if (wordType == stringArrayType)
            {
                return SpeechUnit.WordType.MultipleWord;
            }
            else
            {
                return SpeechUnit.WordType.SingleWord;//заглушка
            }
        }
        public static SpeechUnit.WordType ClassifyVariable(object variable)
        {

            return SpeechUnit.WordType.SingleWord;
        }
    }
}
