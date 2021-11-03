using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Protocols
{
    public class ProtocolProcessor
    {
        public void ProcessRecognizedProtocol(Protocol protocol,sSpeechPattern speechPattern)
        {
            var variables = VariableExtractor.ExtractVariables(protocol.GetPattern(), speechPattern);//мы получили переменные
            //парсим команды
            //вставляем переменные в аргументы
            //выставляем команды в очередь на выполнение
        }
    }
}
