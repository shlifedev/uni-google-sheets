using System;
using System.Text;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            byte[] data = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06,0x07,0x08,0x09,0xa,0xb,0xc,0xd, 0xe, 0xf, 0x11, 0x12, 0x13, 0x14, 0x19, 0x1a, 0x1b};
            var encodeByte = Encoding.UTF8.GetString(data);
            Console.WriteLine("=> + " + encodeByte); 
            var encodeByte2 = Encoding.UTF8.GetBytes(encodeByte);
            foreach (var value in encodeByte2)
            {
                Console.WriteLine(value);
            }
        }
    }
}