namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public class Variable
    {
        public readonly string Name;
        public string Content;
        public string LastFilledBy;//эту механику можно использовать как контекст
        public Variable()
        {
            Name = "";
            Content = "";
            LastFilledBy = "constructor";
        }
        public Variable(string name, string content, string filler = "constructor")
        {
            Name = name;
            Content = content;
            LastFilledBy = filler;
        }
    }
}
