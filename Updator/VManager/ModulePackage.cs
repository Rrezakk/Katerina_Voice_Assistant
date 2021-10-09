using System;
using System.Collections.Generic;
using System.Text;
using Updator.DownloadManager;

namespace Updator.VManager
{
    public class ModulePackage
    {
        public string Name = "";//also folder-name in modules
        public string Version = "";
        public string RequiredCoreVersion = "";
        public string Date = "";
        public readonly List<Downloader.TDownloadFile> Files = new List<Downloader.TDownloadFile>();
        public void Log()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"Module name: {Name}");
            Console.WriteLine($"Module Version: {Version}");
            Console.WriteLine($"RequiredCoreVersion: {RequiredCoreVersion}");
            Console.WriteLine($"Module files: ");
            Files.ForEach(file=>{Console.WriteLine(file.FullUnpackingPath);});
            Console.WriteLine("---------------------------------------------");
        }
        public void Fill()
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Module name: ");
            this.Name = Console.ReadLine();
            Console.Write($"Module Version: ");
            this.Version = Console.ReadLine();
            Console.Write($"RequiredCoreVersion: ");
            this.RequiredCoreVersion = Console.ReadLine();
            Console.WriteLine($"Module sirectory: ");

            while (true)
            {
                var file = new Downloader.TDownloadFile();
                Console.Write($"Archive name: ");
                file.ArchiveName = Console.ReadLine();
                if (file.ArchiveName == "") break;
                Console.Write($"link: ");
                file.Link = Console.ReadLine();
                Console.Write($"Unpacking path: ");
                file.FullUnpackingPath = Console.ReadLine();
            }
            Console.WriteLine("---------------------------------------------");
        }
        public void Fill(string installationPath)
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Module name: ");
            this.Name = Console.ReadLine();
            Console.Write($"Module Version: ");
            this.Version = Console.ReadLine();
            Console.Write($"RequiredCoreVersion: ");
            this.RequiredCoreVersion = Console.ReadLine();
            var date = DateTime.Now;
            this.Date = date.ToShortDateString();
            Console.WriteLine($"Date: {this.Date}");
            Console.WriteLine($"Module directory: {installationPath}");
            var file = new Downloader.TDownloadFile();
            var archiveName = CorePackage.GetArchiveNameFromFullPath(installationPath);
            Console.WriteLine($"    Archive name: {archiveName}");
            file.ArchiveName = archiveName;
            Console.Write($"    link: ");
            file.Link = Console.ReadLine();
            Console.Write($"    Unpacking path: {installationPath}");
            file.FullUnpackingPath = installationPath;
            Console.WriteLine("---------------------------------------------");
        }
    }
}
