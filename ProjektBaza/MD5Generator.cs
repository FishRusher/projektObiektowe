using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjektBaza
{
    class MD5Generator
    {
        public static string FromString(string s)
        {
            var encoder = MD5.Create();
            byte[] tmp = Encoding.ASCII.GetBytes(s);
            byte[] bytes = encoder.ComputeHash(tmp);

            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("x2"));
            }

            return result.ToString();
        }
    }
}
