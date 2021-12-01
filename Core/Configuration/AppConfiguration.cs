using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace K3NA_Remastered_2.Configuration
{
    public class AppConfiguration
    {
        public AppConfiguration(){}
        public AppConfiguration(string source)
        {
            if (source.Contains('.'))
            {
                this.LoadSingle(source);
            }
            else
            {
                this.LoadMultiple(source);
            }    
        }
        private void LoadMultiple(string source = "/Configuration/")
        {
            var files = Directory.GetFiles(source);
            foreach (var file in files)
            {
                if(file.Contains(".env"))
                    LoadSingle(file);
            }
        }
        private readonly Dictionary<string, string> _variablesDictionary = new Dictionary<string, string>();
        public void LoadSingle(string source = "/Configuration/defaultConfig.env")
        {
            Console.WriteLine($"Loading variables:");
            var lines = File.ReadAllLines(source);
            foreach (var line in lines)
            {
                var eqIndex = line.IndexOf('=');
                var variableName = line[..eqIndex];
                eqIndex++;
                var variableContent = line.Substring(eqIndex, line.Length - eqIndex);
                _variablesDictionary.Add(variableName,variableContent);
                Console.WriteLine($"{variableName} = {variableContent}");
            }
        }
        public string GetVariable(string variableName)
        {
            foreach (var variable in _variablesDictionary.Where(variable => variable.Key == variableName))
                return variable.Value;
            return "";
        }
    }
}
