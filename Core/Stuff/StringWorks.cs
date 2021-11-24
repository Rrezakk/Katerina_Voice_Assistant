using System;
using System.Collections.Generic;
using System.Text;

namespace K3NA_Remastered_2.Stuff
{
    public static class StringWorks
    {
        public static string Concat(this string[] a,char delim=';') => string.Join(delim, a);//extension
    }
}
