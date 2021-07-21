using Hamster.ZG;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.IO.FileWriter;
using UGS.Protocol.v2.Req;
using System;

namespace ConsoleApp1
{
    class Program
    {

 
        static void Main(string[] args)
        {
            GoogleSheet.Initialize("https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHzgiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec", "123123");
            GoogleSheet.Generate("1BXya0YQq980kbNBN_-hQAvmBrNkHFIoqXJkQTXIsXHQ");
        }

 
    }
}
