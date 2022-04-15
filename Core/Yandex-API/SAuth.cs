using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace K3NA_Remastered_2.Yandex_API
{
    internal static class SAuth
    {
        public class TokenInfo
        {
            public string IamToken { get; set; }
            public string ExpiresAt { get; set; }
        }
        public static string FolderId => Core.AppConfiguration.GetVariable("FolderId"); 
        public static string OauthToken => Core.AppConfiguration.GetVariable("OauthToken");
        private static TokenInfo _iam;
        public static TokenInfo AccessToken
        {
            get { return _iam ??= GetToken(OauthToken).Result; }
        }
        private static async Task<TokenInfo> GetToken(string oauth)
        {
            var client = new HttpClient();
            var bytes = Encoding.ASCII.GetBytes("{\"yandexPassportOauthToken\":\"" + oauth + "\"}");
            HttpContent ctx = new ByteArrayContent(bytes);
            var response = await client.PostAsync($"https://iam.api.cloud.yandex.net/iam/v1/tokens", ctx);
            if (response.StatusCode != HttpStatusCode.OK) return new TokenInfo();
            TextReader t = new StringReader(response.Content.ReadAsStringAsync().Result);
            JsonReader rd = new JsonTextReader(t);
            var ser = JsonSerializer.CreateDefault();
            var o = ser.Deserialize<TokenInfo>(rd);
            return o;
        }
    }
}
