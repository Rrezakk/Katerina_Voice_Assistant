using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff
{
    class BackgroundProtocol
    {
        public string Name;
        public string Pattern;//just for now, later will be objects
        public string Commands;//just for now, later will be objects
        public BackgroundProtocol(ParsedProtocol protocol)
        {
            this.Name = protocol.Name;
            this.Pattern = protocol.Pattern;
            this.Commands = protocol.Commands;
        }
    }
}
