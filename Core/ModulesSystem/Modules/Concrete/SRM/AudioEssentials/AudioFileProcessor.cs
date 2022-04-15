using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;

namespace K3NA_Remastered_2.ModulesSystem.Modules.Concrete.SRM.AudioEssentials
{
    public static class AudioFileProcessor
    {
        private static int _pathCtr;
        private static string GenerateNewPath()
        {
            return $"{_pathCtr++}.wav";
        }
        public static string SaveWav(short[] audio)
        {
            var binaryBuffer = ToBinary(audio, 0, audio.Length);
            var reader = new RawSourceWaveStream(binaryBuffer, 0, binaryBuffer.Length,
                new WaveFormat(48000, 16, 1));
            using var convertedStream = WaveFormatConversionStream.CreatePcmStream(reader);
            var path = GenerateNewPath();
            WaveFileWriter.CreateWaveFile(path, convertedStream);
            return path;
        }
        public static string WavToOpus(string path, int bitrate = 64)
        {
            var outPath = path.Split('.')[0] + ".ogg";
            if (File.Exists(outPath)) File.Delete(outPath);

            var psi = new ProcessStartInfo
            {
                FileName = "ThirdParty/opusenc.exe",
                Arguments = $"--bitrate {bitrate} --quiet {path} {outPath}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var process = Process.Start(psi))
            {
                process?.StandardError.ReadToEnd();
                process?.StandardOutput.ReadToEnd();
            }
            File.Delete(path);
            return outPath;
        }
        private static byte[] ToBinary(IReadOnlyList<short> arri, int offset, int lenSig)
        {
            var arrb = new byte[lenSig * 2 - offset * 2];
            var len = 0;
            for (var i = offset; i < lenSig; i++)
            {
                var bytes = BitConverter.GetBytes(arri[i]);
                arrb[len++] = bytes[0];
                arrb[len++] = bytes[1];
            }
            return arrb;
        }
    }
}