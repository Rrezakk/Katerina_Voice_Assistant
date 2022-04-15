using System.Text;

namespace K3NA_Remastered_2.LanguageExtensions
{
    public static class StringExtensions
    {
        public static string Repeat(this string str, int count)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }
        public static string Concat(this string[] a,char delim=';') => string.Join(delim, a);//extension
    }
}