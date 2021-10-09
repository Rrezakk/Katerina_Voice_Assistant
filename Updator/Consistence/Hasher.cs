using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Updator.Consistence
{
    public static class Hasher
    {
        private static readonly MD5 _md5 = MD5.Create();
        public static string HashFile(string filename)
        {
            using FileStream stream = File.OpenRead(filename);
            return _md5.ComputeHash(stream).ToString();
        }
    
    }
}
