using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Updator.DownloadManager
{
    public static class Downloader
    {
        public static bool DownloadOrUnpackFile(string script, TDownloadFile file)
        {
            if (File.Exists(file.ArchiveName) && !File.Exists(file.FullUnpackingPath))
            {
                Console.WriteLine($"File exists, unpacking...");
                Unpack(file.ArchiveName, file.FullUnpackingPath);
                return true;
            }
            Console.WriteLine($"File not exists, downloading...");
            var psi = new ProcessStartInfo
            {
                FileName = "python.exe",
                Arguments = $"\"{script}\" \"{file.Link}\" \"{file.ArchiveName}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true

            };
            var errors = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            Console.WriteLine($"errors: {errors}");
            Console.WriteLine($"File downloaded, unpacking...");
            Unpack(file.ArchiveName, file.FullUnpackingPath);
            return true;

        }
        private static void Unpack(string inputPath, string outputPath)
        {
            Console.WriteLine($"Unpacking {inputPath}");
            //if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            var psi = new ProcessStartInfo
            {
                FileName = "tar.exe",
                Arguments = $"-xf \"{inputPath}\"",/* -C {outputPath}*/
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            var errors = "";
            var results = "";
            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }
            Console.WriteLine($"Unpacking errors: {errors}");
            Console.WriteLine($"Moving unpacked folder to destination: {outputPath}");
            try
            {
                MoveExtractedFolder(inputPath.Substring(0, inputPath.Length - 4), outputPath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Moving file error: {e}");
            }
            
        }
        private static void MoveExtractedFolder(string inputPath, string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            foreach (var path in Directory.GetFiles(inputPath, "*.*", SearchOption.AllDirectories))
            {
                if (path.Contains("."))
                {
                    var pth = path.Replace(inputPath, outputPath);
                    var i = pth.LastIndexOf("\\", StringComparison.Ordinal);
                    var dir = pth.Substring(0, i);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    Console.Write ($"Processed: {dir} ");
                }
                Console.WriteLine($"At: {path}");
                File.Copy(path, path.Replace(inputPath, outputPath),true);
            }
            Console.WriteLine($"Removing temporary folder: {inputPath}");
            RemoveTemporaryDirectory(inputPath);
            //Directory.Move(inputPath, outputPath);
        }
        private static void RemoveTemporaryDirectory(string path)
        {
            if(Directory.Exists(path))
            {
                Directory.Delete(path,true);
            }
        }
        public struct TDownloadFile//for downloading archives
        {
            public string ArchiveName;//.zip
            public string Link;
            public string FullUnpackingPath;
        }
        public struct TFolderFile//for checking files existance
        {
            public string Path;
            public string ExpectedHash;
        }
    }
}
