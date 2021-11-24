using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.Yandex_API
{
    public class STT
    {
        public static async Task<string> RecognizeTask(string iam, string folderId, string filepath)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + iam);
            string[] ft = new string[]{"topic=general",
                $"folderId={folderId}",
                "lang=ru-RU"};
            var req = String.Join('&', ft);
            HttpContent ctx = new ByteArrayContent(await File.ReadAllBytesAsync(filepath));
            var response = await client.PostAsync($"https://stt.api.cloud.yandex.net/speech/v1/stt:recognize?{req}", ctx);
            var responseBytes = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBytes);
            return responseBytes;
        }
    }
}
