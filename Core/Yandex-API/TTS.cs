using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Concentus.Oggfile;
using Concentus.Structs;
using NAudio.Wave;

namespace K3NA_Remastered_2.Yandex_API
{
    internal static class Tts
    {
        public const string AudioFilesChachePath = "/TTS-chache/";
        private static async Task<string> SynthPhraseAsync(string text, string filePath, string token, string folderId, string voice = "jane")
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var values = new Dictionary<string, string>
            {
                {"text", text},
                {"lang", "ru-RU"},
                {"folderId", folderId},
                {"voice", voice},
                {"emotion", "neutral"}, //good evil neutral
                {"speed", "1.4"}, //0.1-3.0
                {"format", "oggopus"},
                {"sampleRateHertz", "48000"}
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize", content);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            await using var pcmStream = new MemoryStream();
            var decoder = OpusDecoder.Create(48000, 1);
            var oggIn = new OpusOggReadStream(decoder, new MemoryStream(responseBytes));
            while (oggIn.HasNextPacket)
            {
                var packet = oggIn.DecodeNextPacket();
                if (packet == null) continue;
                foreach (var t in packet)
                {
                    var bytes = BitConverter.GetBytes(t);
                    pcmStream.Write(bytes, 0, bytes.Length);
                }
            }

            pcmStream.Position = 0;
            var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(48000, 1));
            var sampleProvider = wavStream.ToSampleProvider();
            WaveFileWriter.CreateWaveFile16(filePath, sampleProvider);
            return filePath;
        }
        public static string SynthPhraseAndSaveAudioFile(string text, string filePath)
        {
            return SynthPhraseAsync(text,filePath, SAuth.AccessToken.IamToken, SAuth.FolderId).Result;
        }
    }
}