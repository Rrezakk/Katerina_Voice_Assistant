using System;
using System.Collections.Generic;
using System.Text;
using K3NA_Remastered_2.Modules.PerformerStuff.Protocols;
using K3NA_Remastered_2.Modules.RecognitionEngine;

namespace K3NA_Remastered_2.Modules.PerformerStuff.Protocols
{
    public class StandardProtocol
    {
        public string Name;
        public SpeechPattern Pattern;//just for now, later will be objects
        public string Commands;//just for now, later will be objects
        public StandardProtocol(){}
        public StandardProtocol(ParsedProtocol protocol)
        {
            this.Name = protocol.Name;
            this.Pattern = ParsePattern(protocol.Pattern);
            this.Commands = ParseCommands(protocol.Commands);
        }
        private static SpeechPattern ParsePattern(string pattern)
        {
            //parsing to speechUnits
            return new SpeechPattern();
        }
        private static string ParseCommands(string commands)
        {
            return commands;
        }
    }
}
