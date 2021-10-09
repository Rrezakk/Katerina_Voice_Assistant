using System;
using System.Collections.Generic;
using System.Text;
using Updator.DownloadManager;

namespace Updator.VManager
{
    public class CorePackage
    {
        public string Version = "2.3.6";
        public bool IsMajor = false;
        public readonly List<Downloader.TDownloadFile> DownloadFiles = new List<Downloader.TDownloadFile>();
        public List<Downloader.TFolderFile> AllFiles = new List<Downloader.TFolderFile>();
        public readonly List<string> RemovingDirectories = new List<string>(){};//default removing directories
        public string Comment = "comment";
        public string Date = "dd.mm.yyyy";
        public string ExecutivePath = "";
        public void Log()
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Version: {Version}");
            Console.WriteLine($"IsMajor: {IsMajor}");
            Console.WriteLine($"Downloading Files: ");
            foreach (var dfile in DownloadFiles)
            {
                Console.WriteLine($"    {dfile.ArchiveName}");
            }
            Console.WriteLine($"AllFiles: ");
            foreach (var link in AllFiles)
            {
                Console.WriteLine($"    {link.Path}");
            }
            Console.WriteLine($"Executive path: {ExecutivePath}");
            Console.WriteLine($"Comment: {Comment}");
            Console.WriteLine($"Date: {Date}");
            Console.WriteLine("-------------------------------------");
        }
        public void Fill(List<Downloader.TFolderFile> files)
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Version: ");
            this.Version = Console.ReadLine();
            Console.Write($"IsMajor: ");
            this.IsMajor = !string.IsNullOrEmpty(Console.ReadLine());
            Console.Write($"Comment: ");
            this.Comment = Console.ReadLine();
            var date = DateTime.Now;
            this.Date = date.ToShortDateString();
            Console.WriteLine($"Date: {this.Date}");
            Console.Write($"ExecutivePath: ");
            this.ExecutivePath = Console.ReadLine();
            Console.WriteLine("Files for downloading");
            while (true)
            {
                var file = new Downloader.TDownloadFile();
                Console.Write($"    Link: ");
                file.Link = Console.ReadLine();
                if (string.IsNullOrEmpty(file.Link)) break;
                Console.Write($"    ArchiveName: ");
                file.ArchiveName = Console.ReadLine();
                Console.Write($"    FullUnpackingPath: ");
                file.FullUnpackingPath = Console.ReadLine();
                DownloadFiles.Add(file);
            }

            Console.WriteLine($"Files: ");
            AllFiles = files;
            AllFiles.ForEach(file => { Console.WriteLine(file.Path); });
            Console.WriteLine("---------------------------------------------");
        }

        public void Fill(List<Downloader.TFolderFile> files, string installationPath)
        {
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Version: ");
            this.Version = Console.ReadLine();

            Console.Write($"IsMajor: ");
            this.IsMajor = !string.IsNullOrEmpty(Console.ReadLine());

            Console.Write($"Comment: ");
            this.Comment = Console.ReadLine();
            
            var date = DateTime.Now;
            this.Date = date.ToShortDateString();
            Console.WriteLine($"Date: {this.Date}");

            Console.Write($"ExecutivePath: ");
            this.ExecutivePath = Console.ReadLine();

            Console.WriteLine("Path's for removing directories: ");
            while (true)
            {
                Console.Write($"    Path: ");
                var path = Console.ReadLine();
                if (string.IsNullOrEmpty(path)) break;
                this.RemovingDirectories.Add(path);
            }

            Console.WriteLine("File for downloading:");
            var file = new Downloader.TDownloadFile();
            Console.Write($"    Enter link: ");
            file.Link = Console.ReadLine();

            var archivename = GetArchiveNameFromFullPath(installationPath);
            Console.WriteLine($"    ArchiveName: {archivename}");
            file.ArchiveName = archivename;

            Console.WriteLine($"    FullUnpackingPath: {installationPath}");
            file.FullUnpackingPath = installationPath;
            DownloadFiles.Add(file);
            

            Console.WriteLine($"Files: ");
            AllFiles = files;
            AllFiles.ForEach(folderFile => { Console.WriteLine(folderFile.Path); });
            Console.WriteLine("---------------------------------------------");
        }

        public static string GetArchiveNameFromFullPath(string installationPath)
        {
            var i = installationPath.LastIndexOf("\\", StringComparison.Ordinal) + "\\".Length;
            var c = installationPath.Length - i;
            var archivename = installationPath.Substring(i, c) + ".zip";
            return archivename;
        }
    }
}
