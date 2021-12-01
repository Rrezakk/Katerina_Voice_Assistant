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
        public static void Test()
        {
            var eaStopwatch = new Stopwatch();
            eaStopwatch.Start();
            SpeakAsync("текст");
            Console.WriteLine($"In: {eaStopwatch.ElapsedMilliseconds}ms");
            eaStopwatch.Stop();
        }
        public static async void SpeakAsync(string text)
        {
            await Task.Run(() => Synth(SAuth.AccessToken.IamToken, SAuth.FolderId, text));
        }
        private static async void Synth(string token, string folderId, string text)
        {
            try
            {
                await Task.Run(() => Synthesis(token, folderId, text));
                Console.WriteLine($"Synthesis complete");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Synthesis error: {e}");
            }
        }
        private static async void Synthesis(string token, string folderId, string text, string voice = "jane")
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
                {"speed", "1.0"}, //0.1-3.0
                {"format", "oggopus"},
                {"sampleRateHertz", "48000"}
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize", content);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            const string fileWav = "Audio.wav";
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
            WaveFileWriter.CreateWaveFile16($"{fileWav}", sampleProvider);
            //await File.WriteAllBytesAsync("speech.ogg", responseBytes);
            //запуск звука
            var player = new System.Media.SoundPlayer(@"Audio.wav");
            player.Play();
        }
    }
}