using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Updator.DownloadManager;

namespace Updator.Consistence
{
    internal static class ConsistenceManager
    {
        private static IEnumerable<string> GetFilesInDirectory(string pathToFolder)=>Directory.EnumerateFiles(pathToFolder,"*",SearchOption.AllDirectories);
        public static List<Downloader.TFolderFile> GetTFolderFiles(string folder)//hashing  - checking                    ---for prepairing
        {
            var paths = GetFilesInDirectory(folder);
            return paths.Select(path => new Downloader.TFolderFile() {ExpectedHash = Hasher.HashFile(path), Path = path}).ToList();
        }
        public static bool CheckAndDownloadFolders(List<Downloader.TDownloadFile> folders)
        {
            try
            {
                for (var i = 0; i < folders.Count; i++)
                {
                    var fav = folders[i];
                    if (File.Exists(fav.FullUnpackingPath)) continue;
                    Console.WriteLine($"Downloading file {i+1}/{folders.Count}, cause not exists: " + fav.FullUnpackingPath);
                    if (!Downloader.DownloadOrUnpackFile(Program.PythonScriptPath, fav))
                    {
                        throw new Exception(
                            "Something went wrong with downloading! Try to check you internet connection");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"CheckAndDownloadFolders error: {e}");
                return false;
            }

            return true;
        }//first
        public static bool CheckInstalledFiles(List<Downloader.TFolderFile> files)
        {
            try
            {
                List<Downloader.TDownloadFile> folders = new List<Downloader.TDownloadFile>();
                for (int i = 0; i < files.Count; i++)
                {
                    var f = files[i];
                    if (f.ExpectedHash != Hasher.HashFile(f.Path))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"CheckInstalledFiles exception: {e}");
                return false;
            }
            
        }//second
    }
}
