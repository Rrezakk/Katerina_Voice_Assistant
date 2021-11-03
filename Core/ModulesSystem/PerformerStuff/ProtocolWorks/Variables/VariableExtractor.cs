using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Variables;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Variables
{
    public static class VariableExtractor
    {
        public static VariableStorage ExtractVariables(PSpeechPattern proto,sSpeechPattern speech)
        {
            return new VariableStorage(new List<Variable>());
        }
    }
}
