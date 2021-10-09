using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Updator.Consistence;
using Updator.Cryptography;
using Updator.DownloadManager;
using static Updator.Cryptography.Crypto;

namespace Updator.VManager
{
    class VersionManager
    {
        public VersionManager()
        {
            CheckInstallation();
        }

        private string _currentVersion = "0.0.0";

        private static bool CompareVersionStrings(string current,string last)
        {
            var res = StringVersionToInt(last) > StringVersionToInt(current);
            Console.WriteLine($"Versions: server:{last} client: {current} Update needed: {res}");
            return res;
        }
        private static int StringVersionToInt(string current)
        {
            var cur1 = current.Substring(0, current.IndexOf("."));
            var cur2 = current.Substring(current.IndexOf(".") + 1, current.IndexOf(".", 2) - current.IndexOf(".") - 1);
            var cur3 = current.Substring(current.IndexOf(".", 2) + 1, current.Length - current.IndexOf(".", 2) - 1);
            int currentInt = Int32.Parse(cur1) * 100 + Int32.Parse(cur2) * 10 + Int32.Parse(cur3);
            return currentInt;
        }
        public string GenerateVersion(bool crypt = true)
        {
            var info = new CorePackage();
            try
            {
                Console.Write("Version: ");
                info.Version = Console.ReadLine();
                Console.Write("IsMajor: ");
                info.IsMajor = Console.ReadLine() != "";
                Console.Write("Comment: ");
                info.Comment = Console.ReadLine();
                Console.Write("Date: ");
                info.Date = Console.ReadLine();
                Console.Write("Enter TFolderFile's (to quit press enter):");
                var inc = 0;
                while (true)
                {
                    Console.WriteLine($"    TFolderFile #{inc++}: ");
                    Console.Write("     Filename with extension of archive:");
                    var filename = Console.ReadLine();
                    if (filename == "") break;
                    Console.Write("     Link: ");
                    var link = Console.ReadLine();
                    Console.Write("     Installation path: ");
                    var path = Console.ReadLine();
                    Console.Write("     Archive name: ");
                    var aname = Console.ReadLine();
                    Downloader.TDownloadFile tmp = new Downloader.TDownloadFile
                    {
                        FullUnpackingPath = path, Link = link, ArchiveName = aname
                    };
                    info.DownloadFiles.Add(tmp);
                }

                Console.Write($"Filling files from path: ");
                var fillingPath = Console.ReadLine();
                info.AllFiles = ConsistenceManager.GetTFolderFiles(fillingPath);
                var ans = JsonConvert.SerializeObject(info);
                return crypt ? Encrypt(ans) : ans;
            }
            catch
            {
                Console.WriteLine(JsonConvert.SerializeObject(info));
                throw;
            }
        }

        private static List<T> FetchVersions<T>(string link, bool crypted = true)
        {
            var versions = new List<T>();
            var request = WebRequest.Create(link);
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream!);
            while (true)
            {
                var responseFromServer = reader.ReadLine();
                if (string.IsNullOrEmpty(responseFromServer))
                    break;
                var res = "";
                if (crypted)
                    res = Decrypt(responseFromServer);
                var info = (T)JsonConvert.DeserializeObject<T>(res);
                versions.Add(info);
            }
            return versions;
        }
        public static List<string> FetchModule(string link, bool crypted = false)
        {
            var lines = new List<string>();
            var request = WebRequest.Create(link);
            var response = request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream!);
            
                var responseFromServer = reader.ReadToEnd();
                Console.WriteLine($"Resp: {responseFromServer}");
                var res = "";
                if (crypted)
                    res = Decrypt(responseFromServer);
                lines.Add(res);
            
            return lines;
        }
        public void CreateCorePackage()
        {
            var package = new CorePackage();
            Console.Write($"Enter folder path: ");
            package.Fill(ConsistenceManager.GetTFolderFiles(Console.ReadLine()));
            var str = JsonConvert.SerializeObject(package);
            Console.WriteLine(Crypto.Encrypt(str));
            Console.ReadKey();
        }
        public void CreateCorePackage(bool enableAutoFilling)
        {
            //if path is defined, installation will be in path folder
            var package = new CorePackage();
            Console.Write($"Enter folder path: ");
            var path = Console.ReadLine();
            if (enableAutoFilling)
                package.Fill(ConsistenceManager.GetTFolderFiles(path), path);
            else
                package.Fill(ConsistenceManager.GetTFolderFiles(path));
            var str = JsonConvert.SerializeObject(package);
            Console.WriteLine(Crypto.Encrypt(str));
            Console.ReadKey();
        }
        public void GetCore(string corePackageInfoLink)
        {
            Console.WriteLine("Getting Core...");
            var corePackage = FetchVersions<CorePackage>(corePackageInfoLink, true);
            var lastPackage = corePackage.Last();
            Console.WriteLine("Last version: ");
            lastPackage.Log();
            if (CompareVersionStrings(_currentVersion, lastPackage.Version)/*|| ConsistenceManager.CheckInstalledFiles(lastPackage.AllFiles)*/)
            {
                Console.WriteLine("Update needed");
                Console.WriteLine("Removing directories...");
                foreach (var directory in lastPackage.RemovingDirectories)
                {
                    Directory.Delete(directory, true);
                }
                Console.WriteLine("Downloading files...");
                ConsistenceManager.CheckAndDownloadFolders(lastPackage.DownloadFiles);
            }
            else
            {
                Console.WriteLine("No need to update");
            }
            SetVersionAfterUpdating(lastPackage.Version);
            //version recording lastPackage.version
            Console.WriteLine("Installed");
            Process.Start(lastPackage.ExecutivePath);
            Console.WriteLine("Launched");
        }

        public void CreateModulePackage()
        {
            var package = new ModulePackage();
            package.Fill();
            var packageString = JsonConvert.SerializeObject(package);
            Console.WriteLine(Crypto.Encrypt(packageString));
            Console.ReadKey();
        }
        public void GetModules(IEnumerable<string> modules, string modulesRepoLink)
        {
            foreach (var moduleName in modules)
            {
                var moduleVersionInfoLink = modulesRepoLink + moduleName + ".txt";
                GetModule(moduleVersionInfoLink, moduleName);
            }
        }
        private static void GetModule(string link, string moduleName)
        {
            try
            {
                var modulePackages = FetchVersions<ModulePackage>(link, true);
                var lastModulePackage = modulePackages.Last();
                if (CompareVersionStrings("0.0.1", lastModulePackage.Version))
                {
                    //update needed
                    //check for core minimum version
                }
                else
                {
                    //no need to be updated
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Couldn't get module \"{moduleName}\" cos: {e}");
            }
        }

        private static void SetVersionAfterUpdating(string version,string katerinaInstallationFolder = Program.KaterinaInstallationFolder, string versionFileRelationalPath = Program.VersionFileRelationalPath)
        {
            var path = katerinaInstallationFolder + versionFileRelationalPath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path,version);
        }

        private void CheckInstallation(string katerinaFolder = Program.KaterinaInstallationFolder, string versionFileR = Program.VersionFileRelationalPath,string modulesDirR = Program.ModulesRelationalPath)//with KaterinaInstallationFolder and relative paths
        {
            Console.WriteLine("Checking installation directory...");
            if (Directory.Exists(katerinaFolder))
            {
                Console.WriteLine($"Main directory exists!");
            }
            else
            {
                Console.WriteLine($"Main directory not exists! Creating...");
                Directory.CreateDirectory(katerinaFolder);
            }
            var modulesAbsDir = katerinaFolder + modulesDirR;
            if (!Directory.Exists(modulesAbsDir))
            {
                Directory.CreateDirectory(modulesAbsDir);
                Console.WriteLine($"Created dir: {modulesAbsDir}");
            }
            try
            {
                _currentVersion = File.ReadAllText(katerinaFolder + versionFileR);
            }
            catch (Exception e)
            {
                _currentVersion = "0.0.0";
            }
            
            Console.WriteLine($"Current version: {_currentVersion}");
        }
    }
}
