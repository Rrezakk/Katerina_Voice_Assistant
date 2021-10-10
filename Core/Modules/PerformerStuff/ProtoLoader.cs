using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace K3NA_Remastered_2.Modules.PerformerStuff
{
    public static class ProtoLoader
    {
        private static class Config{public const string ProtocolsPath = "StandardProtocols\\"; }
        private static IEnumerable<string> GetProtocolLines(string path)
        {
            var lines = File.ReadAllLines(path);
            var spacesCount = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim(' ');
                if (lines[i] == "")
                    spacesCount++;
            }
            if (spacesCount == 0) return lines;//no empty lines

            var result = new string[lines.Length-spacesCount];
            var j = 0;
            foreach (var t in lines)
            {
                if (t!="")
                {
                    result[j++] = t;
                }
            }
            return result;
        }
        private static string[] GetAllProtocolsLines(IEnumerable<string> paths)
        {
            var lines = new List<string>();
            foreach (var path in paths)
            {
                var tmp = GetProtocolLines(path);
                lines.AddRange(tmp);
            }
            return lines.ToArray();
        }
        private static string[] GetProtocolsPaths(string folder)
        {
            var files = Directory.GetFiles(folder, "*.txt");
            return files;
        }
        private static List<string> SplitProtocols(string[] lines)
        {
            //starts: Protocol:
            //ends: };
            var result = new List<string>();
            var offset = 0;
            while (offset<lines.Length)
            {
                var start = 0;
                for (var i = offset; i < lines.Length; i++)//just one iteration
                {
                    var line = lines[i];
                    if (line.Contains("Protocol:"))
                    {
                        start = i;
                        Console.WriteLine($"    Found protocol starts at: {i}");//debug
                    }

                    if (line.Contains("};"))
                    {
                        Console.WriteLine($"    Ends at: {i}"); //debug
                        var end = i;
                        var ctr = 0; //debug
                        var protocolStrings = "";
                        var j = -1;
                        while(start + j++!=end)
                        {
                            ctr++;
                            protocolStrings += lines[start + j];
                        }
                        Console.WriteLine($"    Reserves {ctr} lines"); //debug
                        result.Add(protocolStrings);
                        offset = ++end;
                        break;
                    }
                }
            }
            return result;
        }
        private static string GetProtocolBlock(string protocol, string qualifier,string ends)
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
        private static string GetProtocolBlock(string protocol, string qualifier,string ends,out string parameter)
        {
            var qualifier2 = qualifier.Substring(0, qualifier.Length - 1)+"{";
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
                var k = protocol.IndexOf("{",j, StringComparison.InvariantCulture)+"{".Length;
                var m = protocol.IndexOf(/*"}"*/ends, k, StringComparison.InvariantCulture);
                return protocol[k..m];
            }
        }
        //private static string GetProtocolType(string protocol)
        //{
        //    var i = protocol.IndexOf("Protocol:",StringComparison.InvariantCulture) + "Protocol:".Length;
        //    var j = protocol.IndexOf("{", i, StringComparison.InvariantCulture);
        //    return protocol.Substring(i, j - i);
        //}
        //private static string GetProtocolPattern(string protocol)
        //{
        //    var qualifierLength = "Pattern{".Length;
        //    var i = protocol.IndexOf("Pattern{",StringComparison.InvariantCulture);
        //    if (i == - 1)
        //    {
        //        return "none";
        //    }
        //    i += qualifierLength;
        //    var j = protocol.IndexOf("}", i, StringComparison.InvariantCulture);
        //    return protocol.Substring(i, j - i);
        //}
        //private static string GetProtocolCommands(string protocol)
        //{

        //}
        public static void LoadProtocols(string folder= Config.ProtocolsPath)
        {
            Console.WriteLine("Loading protocols");
            var protocols = GetProtocolsPaths(folder);
            Console.WriteLine($"Protocol files found: {protocols.Length}");
            var lines = GetAllProtocolsLines(protocols);
            Console.WriteLine($"Lines total: {lines.Length}");
            var splittedProtocols = SplitProtocols(lines);
            Console.Write($"Protocols founded total: ");
            splittedProtocols.ForEach(Console.WriteLine);
            var parsedProtocols = new List<ParsedProtocol>();
            Console.WriteLine($"-------------------Parsing---------------------");
            for (var i = 0; i < splittedProtocols.Count; i++)
            {
                var protocol = splittedProtocols[i];
                Console.WriteLine($"--------------{i}--------------");
                var type = GetProtocolBlock(protocol, "Protocol:", "{");
                var pattern = GetProtocolBlock(protocol, "Pattern{", "}");
                var commands = GetProtocolBlock(protocol, "Commands{", "}");
                var statement = GetProtocolBlock(protocol,"Statement:","}", out var statementType);
                var name = GetProtocolBlock(protocol, "Name{", "}");

                Console.WriteLine($"Protocol name: {name}");
                Console.WriteLine($"Protocol type: {type}");
                Console.WriteLine($"Protocol pattern: {pattern}");
                Console.WriteLine($"Protocol commands: {commands}");
                Console.WriteLine($"Protocol statement: {statement} Type: {statementType}");
                Console.WriteLine("-----------------------------");
                ParsedProtocol parsedProtocol = new ParsedProtocol(name,type,commands,pattern,statement,statementType);
                parsedProtocols.Add(parsedProtocol);
            }
            var standardProtocols = new List<StandardProtocol>();
            var backgroundProtocols = new List<BackgroundProtocol>();
            foreach (var protocol in parsedProtocols)
            {
                switch (protocol.Type)
                {
                    case "standard":
                        standardProtocols.Add(new StandardProtocol(protocol));
                        break;
                    case "background":
                        backgroundProtocols.Add(new BackgroundProtocol(protocol));
                        break;
                }
            }
            //now have standardProtocols and backgroundProtocols -> let's work with it!
        }

    }
}
