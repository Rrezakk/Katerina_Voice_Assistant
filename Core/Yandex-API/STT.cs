using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace K3NA_Remastered_2.Yandex_API
{
    // ReSharper disable once InconsistentNaming
    public static class STT
    {
        public static async Task<string> RecognizeTask(string iam, string folderId, string filepath)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + iam);
            var ft = new string[]{"topic=general",
                $"folderId={folderId}",
                "lang=ru-RU"};
            var req = string.Join('&', ft);
            HttpContent ctx = new ByteArrayContent(await File.ReadAllBytesAsync(filepath));
            var response = await client.PostAsync($"https://stt.api.cloud.yandex.net/speech/v1/stt:recognize?{req}", ctx);
            var responseBytes = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseBytes);
            return responseBytes;
        }
    }
}
