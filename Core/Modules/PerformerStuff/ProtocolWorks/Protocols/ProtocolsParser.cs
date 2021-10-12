using System;
using System.Collections.Generic;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    public class ProtocolsParser
    {
        public static SpeechPattern ParsePattern(string pattern)
        {
            throw new NotImplementedException();
        }
        public static List<Command> ParseCommands(string pattern)
        {
            throw new NotImplementedException();
        }
        public static string GetProtocolBlock(string protocol, string qualifier, string ends)
        {
            var i = protocol.IndexOf(qualifier, StringComparison.InvariantCulture);
            if (i == -1)
            {
                return "none";
            }
            i += qualifier.Length;
            var j = protocol.IndexOf(ends, i, StringComparison.InvariantCulture);
            return protocol.Substring(i, j - i);
        }
        public static string GetProtocolBlock(string protocol, string qualifier, string ends, out string parameter)
        {
            var qualifier2 = qualifier[0..^1] + "{";
            var i = protocol.IndexOf(qualifier, StringComparison.InvariantCulture);
            var j = 0;
            if (i == -1)
            {
                parameter = "none";
            }
            else
            {
                i += qualifier.Length;
                j = protocol.IndexOf("{", i, StringComparison.InvariantCulture);
                parameter = protocol[i..j];
            }
            //i,j - indexes of parameter
            if (parameter == "none")
            {
                var k = protocol.IndexOf(qualifier2/*"Statement{"*/, StringComparison.InvariantCulture);
                if (k == -1)
                {
                    return "none";
                }
                k += /*"Statement{"*/qualifier2.Length;
                var m = protocol.IndexOf(/*"}"*/ends, k, StringComparison.InvariantCulture);
                return k >= m ? "none" : protocol[k..m];
            }
            else
            {
                var k = protocol.IndexOf("{", j, StringComparison.InvariantCulture) + "{".Length;
                var m = protocol.IndexOf(/*"}"*/ends, k, StringComparison.InvariantCulture);
                return protocol[k..m];
            }
        }
    }
}
