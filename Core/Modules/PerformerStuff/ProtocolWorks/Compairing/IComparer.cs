using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Compairing
{
    public interface IComparer
    {
        public ICompareResult Compare(object data, object pattern);
    }
}
