using System.Security.Cryptography;
using System.Text;

namespace K3NA_Remastered_2.ModulesSystem.PerformerStuff.FunctionalComponents
{
    internal class Hashing
    {
        public static string ComputeSha256Hash(string rawData)
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
