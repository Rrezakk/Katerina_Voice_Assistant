using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using static System.String;

namespace Updator.Cryptography
{
    public static class Crypto
    {
        public static void ConsoleCrypt()
        {
            Console.WriteLine("Original String: ");
            string originalString = Console.ReadLine();
            string cryptedString = Encrypt(originalString);
            //Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEncrypt Result: {0}", cryptedString);
        }
        static readonly byte[] Bytes = Encoding.ASCII.GetBytes(Program.EncryptionPass);
        public static string Encrypt(string originalString)
        {
            if (IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException
                    ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(Bytes, Bytes), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        public static string Decrypt(string cryptedString)
        {
            if (IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                    ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                (Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(Bytes, Bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
