using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands
{
    public abstract class Command
    {
        public abstract void Execute(object[] arguments);
    }
}
