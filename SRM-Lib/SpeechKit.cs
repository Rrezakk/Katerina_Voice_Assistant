using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace k3na_voice
{
    public static class SpeechKit
    {
        public class TokenInfoResult
        {
            public string IamToken { get; set; }
            public string ExpiresAt { get; set; }
        }
        public static async Task<TokenInfoResult> GetToken(string oauth)
        {
            HttpClient client = new HttpClient();
            //https://iam.api.cloud.yandex.net/iam/v1/tokens
            //curl -d "{\"yandexPassportOauthToken\":\"<OAuth-token>\"}" "https://iam.api.cloud.yandex.net/iam/v1/tokens"
            byte[] bytes = Encoding.ASCII.GetBytes("{\"yandexPassportOauthToken\":\"" + oauth + "\"}");
            HttpContent ctx = new ByteArrayContent(bytes);
            var response = await client.PostAsync($"https://iam.api.cloud.yandex.net/iam/v1/tokens", ctx);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                TextReader t = new StringReader(response.Content.ReadAsStringAsync().Result);
                JsonReader rd = new JsonTextReader(t);
                var ser = JsonSerializer.CreateDefault();
                var o = ser.Deserialize<TokenInfoResult>(rd);
                //Console.WriteLine(o.iamToken);
                return o;
            }
            return new TokenInfoResult();
            //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
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
