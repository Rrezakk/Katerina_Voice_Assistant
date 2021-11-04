using System;
using System.Collections.Generic;
using System.Linq;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Tables;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Commands;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Patterns;
using K3NA_Remastered_2.ModulesSystem.PerformerStuff.ProtocolWorks.Tables;

namespace K3NA_Remastered_2.Modules.PerformerStuff.ProtocolWorks.Protocols
{
    public static class ProtocolsParser
    {
        public static PSpeechPattern ParseProtocolPattern(string pattern)
        {
            var pSpeechPattern = new PSpeechPattern();
            var units = pattern.Split(" ");
            for (var i = 0; i < units.Length; i++){units[i] = units[i].Trim().Trim(',').TrimEnd('.');}
            foreach (var unit in units)
            {
                var pSpeechUnit = new PSpeechUnit(unit);
                pSpeechPattern.AddUnit(pSpeechUnit);
            }
            return pSpeechPattern;
        }
        public static List<Command> ParseCommands(string commands)
        {
            //SayAny("Приветик","скучала по вам!","Здравствуйте, сэр");MakeSomething();
            //Say("<var:name:in/out/auto>","arg2");
            //Console.WriteLine("--------------------------------------------->>>");
            var commandsArr = commands.Split(";");
            //foreach (var c in commandsArr)
            //{
            //  Console.WriteLine($"  Command: {c}");  
            //}
            //Console.WriteLine("--------------------------------------------->>>");
            var result = new List<Command>();
            foreach (var command in commandsArr)
            {
                if (IsNoneOrEmpty(command))
                    continue;   
                
                result.Add(ParseCommandUnit(command));
            }
            return result;
        }
        private static Command ParseCommandUnit(string unit)
        {
            if (String.IsNullOrEmpty(unit)) return null;
            //сначала парсим аргументы, мб не заменяем переменные по ParseTripleUnit
            //Console.WriteLine(unit);
            var i = unit.IndexOf("(", StringComparison.Ordinal);
            var j = unit.LastIndexOf(")", StringComparison.Ordinal);

            var name = unit.Substring(0, i);
            //Console.WriteLine(name);
            var command = CommandsTable.GetConcreteCommand(name);

            var argumentsString = unit.Substring(i + 1, j - i - 1);
            //Console.WriteLine(argumentsString);
            var arguments = argumentsString.Split("\",\"");
            for (var index = 0; index < arguments.Length; index++)
            {
                arguments[index] = arguments[index].Trim('"');
            }

            command.Arguments = arguments;
            command.Name = name;
            return command;
            //filling
            //sending to executor
        }
        private static bool IsNoneOrEmpty(string str)
        {
            return string.IsNullOrEmpty(str) || str == "none";
        }
        public static void ParseTripleUnit(string unit, out string chema, out string type, out string text)
        {
            var i = unit.IndexOf("<", StringComparison.Ordinal) + 1;
            var j = unit.IndexOf(":", StringComparison.Ordinal);
            var k = unit.IndexOf(":", j + 1, StringComparison.Ordinal) - 1;
            var m = unit.IndexOf(">", StringComparison.Ordinal) + ">".Length;
            if (k==-1)
            {
                chema = unit.Substring(i, j - i);
                type = unit.Substring(j+1, m - j - 1);
                text = "";
            }
            else
            {
                chema = unit.Substring(i, j - i);
                type = unit.Substring(j + 1, k - j);
                text = unit.Substring(k + 2, m - k - 3);
            }
           
           
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
        public static string GetProtocolType(string protocol)
        {
            return GetProtocolBlock(protocol, "Protocol:", "{");
        }
    }
}
