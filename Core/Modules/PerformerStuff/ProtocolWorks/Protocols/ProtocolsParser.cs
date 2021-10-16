﻿using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    public static class ProtocolsParser
    {
        public static pSpeechPattern ParseProtocolPattern(string pattern)
        {
            var pSpeechPattern = new pSpeechPattern();
            var units = pattern.Split(" ");
            for (var i = 0; i < units.Length; i++){units[i] = units[i].Trim();}
            foreach (var unit in units)
            {
                var pSpeechUnit = new pSpeechUnit(unit);
                pSpeechPattern.AddUnit(pSpeechUnit);
            }
            return pSpeechPattern;
        }
        public static List<Command> ParseCommands(string pattern)
        {
            throw new NotImplementedException();
        }

        public static void ParsePatternUnit(string unit, out string chema, out string type, out string text)
        {
            var i = unit.IndexOf("<", StringComparison.Ordinal)+1;
            var j = unit.IndexOf(":", StringComparison.Ordinal);
            var k = unit.IndexOf(":", j+1, StringComparison.Ordinal)-1;
            var m = unit.IndexOf(">", StringComparison.Ordinal) + ">".Length;
            chema = unit.Substring(i, j - i);
            type = unit.Substring(j+1, k - j);
            text = unit.Substring(k+2, m - k-3);
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
