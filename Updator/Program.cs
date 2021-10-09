using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Updator.VManager;

namespace Updator
{
    public static class Program
    {
        public const string PythonScriptPath = "DownloadManager/download.py";
        public const string EncryptionPass = "eight888";
        private const string CorePackageInfoLink = "https://raw.githubusercontent.com/Rrezakk/Katerina/main/updates/Core.txt";
        private const string ModulesRepoLink = "https://raw.githubusercontent.com/Rrezakk/Katerina/main/updates/modules/";
        private static readonly string[] Modules = new[]{"srm"};

        public const string KaterinaInstallationFolder = "C:\\K3NA";
        public const string ModulesRelationalPath = "\\Modules\\Standart";
        public const string VersionFileRelationalPath = "\\K3NA.version";

        private static void GetModule(string repo, string name,string modulesFolder="\\")
        {
            var path = modulesFolder + "\\"+ Modules[0] + ".txt";
            var link = ModulesRepoLink + Modules[0] + ".txt";
            var client = new WebClient();
            client.DownloadFile(link, path);
        }
        private static void Main(string[] args)
        {
            var versionManager = new VersionManager();
            var modules = Modules;
            if (modules.Length>0&&modules.First() == "modules")
                modules = args;
            GetModule(ModulesRepoLink,modules[0],KaterinaInstallationFolder+ ModulesRelationalPath);
            Console.ReadLine();
#if CreateVersion
            versionManager.CreateCorePackage(true);
            Console.ReadKey();
#endif
            try { versionManager.GetCore(CorePackageInfoLink);}catch (Exception e) { Console.WriteLine($"Getting core exception: {e}"); }
            //try {GetModules(modules, ModulesRepoLink);} catch (Exception e) { Console.WriteLine($"Getting modules exception: {e}"); }//not working now
        }
    }
}

