using System;
using System.Text;

namespace UGS.Unused
{
    public static class Base64Utils
    {
        public static string Encode(string data)
        {
            byte[] basebyte = System.Text.Encoding.UTF8.GetBytes(data);
            string s64 = Convert.ToBase64String(basebyte);
            return s64;
        }

        public static string Decode(string s64)
        {
            byte[] bytetest = Convert.FromBase64String(s64);
            s64 = Encoding.UTF8.GetString(bytetest);
            return s64;
        }

    }
}
