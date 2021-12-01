using System;
using K3NA_Remastered_2.Yandex_API;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands.ConcreteCommands
{
    internal class SayCommand:Command
    {
        public override string Name { get; set; }
        public override string[] Arguments { get; set; }
        public override void Execute()
        {
            Tts.SpeakAsync(Arguments[0]);
        }
    }
}
