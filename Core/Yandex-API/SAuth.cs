using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace K3NA_Remastered_2.Yandex_API
{
    public class SAuth
    {
        public class TokenInfo
        {
            public string IamToken { get; set; }
            public string ExpiresAt { get; set; }
        }
        public const string FolderId = "b1glia0cgh1tqpjp48f4";
        public const string OauthToken = "AQAAAAAO_IRoAATuwQCcneB1GEPYlK7mFP_wcJg";
        public static TokenInfo AccessToken;
        static SAuth()
        {
            AccessToken = GetToken(OauthToken).Result;
        }
        public static async Task<TokenInfo> GetToken(string oauth)
        {
            HttpClient client = new HttpClient();
            //curl -d "{\"yandexPassportOauthToken\":\"<OAuth-token>\"}" "https://iam.api.cloud.yandex.net/iam/v1/tokens"
            byte[] bytes = Encoding.ASCII.GetBytes("{\"yandexPassportOauthToken\":\"" + oauth + "\"}");
            HttpContent ctx = new ByteArrayContent(bytes);
            var response = await client.PostAsync($"https://iam.api.cloud.yandex.net/iam/v1/tokens", ctx);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                TextReader t = new StringReader(response.Content.ReadAsStringAsync().Result);
                JsonReader rd = new JsonTextReader(t);
                var ser = JsonSerializer.CreateDefault();
                var o = ser.Deserialize<TokenInfo>(rd);
                return o;
            }
            return new TokenInfo();
        }
    }
}
