using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using Concentus.Oggfile;
using Concentus.Structs;

namespace K3NA_Remastered_2.Yandex_API
{
    class TTS
    {
        public static void test()
        {
            Stopwatch eaStopwatch = new Stopwatch();
            eaStopwatch.Start();
            SpeakAsync("текст");
            Console.WriteLine($"In: {eaStopwatch.ElapsedMilliseconds}ms");
            eaStopwatch.Stop();
        }
        public static async void SpeakAsync(string text)
        {
            await Task.Run(() => Synth(SAuth.AccessToken.IamToken, SAuth.FolderId, text));
        }
        public static async void Synth(string token, string folderId, string text)
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
        private static async void Synthesis(string token, string FolderId, string text, string voice = "jane")
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var values = new Dictionary<string, string>
            {
                {"text", text},
                {"lang", "ru-RU"},
                {"folderId", FolderId},
                {"voice", voice},
                {"emotion", "neutral"}, //good evil neutral
                {"speed", "1.0"}, //0.1-3.0
                {"format", "oggopus"},
                {"sampleRateHertz", "48000"}
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize", content);
            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            var fileWav = "Audio.wav";
            await using MemoryStream pcmStream = new MemoryStream();
            OpusDecoder decoder = OpusDecoder.Create(48000, 1);
            OpusOggReadStream oggIn = new OpusOggReadStream(decoder, new MemoryStream(responseBytes));
            while (oggIn.HasNextPacket)
            {
                short[] packet = oggIn.DecodeNextPacket();
                if (packet != null)
                {
                    for (int i = 0; i < packet.Length; i++)
                    {
                        var bytes = BitConverter.GetBytes(packet[i]);
                        pcmStream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            pcmStream.Position = 0;
            var wavStream = new RawSourceWaveStream(pcmStream, new WaveFormat(48000, 1));
            var sampleProvider = wavStream.ToSampleProvider();
            WaveFileWriter.CreateWaveFile16($"{fileWav}", sampleProvider);
            File.WriteAllBytes("speech.ogg", responseBytes);
            //запуск звука
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Audio.wav");
            player.Play();
        }
    }
}