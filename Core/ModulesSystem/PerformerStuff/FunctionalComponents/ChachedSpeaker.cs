using System;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using K3NA_Remastered_2.Yandex_API;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.FunctionalComponents
{
    internal class ChachedSpeaker//to decrease tts api requests
    {
        private static readonly SoundPlayer player = new();
        public static void Speak(string text)
        {
            var chacheDirectory = Environment.CurrentDirectory + Tts.AudioFilesChachePath;
            if (!Directory.Exists(chacheDirectory))
            {
                Directory.CreateDirectory(chacheDirectory);
            }


            var hash = ComputeSha256Hash(text);
            var fileName = hash + ".wav";
            var filePath = chacheDirectory + fileName;


            if (!File.Exists(filePath))
            {
                Tts.SynthPhraseAndSaveAudioFile(text, filePath);
            }

            player.SoundLocation = filePath;
            player.PlaySync();
        }
        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
