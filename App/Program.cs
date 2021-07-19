using Hamster.ZG;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.IO.FileWriter;
using System;

namespace ConsoleApp1
{
    class Program
    {

        public static void Credential()
        {
            HamsterGoogleSheet.Init(new GSParser(), new FileReader());
            GoogleDriveWebRequester.Instance.password = "yourpassword";
            GoogleDriveWebRequester.Instance.baseURL = "https://script.google.com/macros/s/AKfycbxEQl5s-Q0qwSRtUEsJF8FcWlrPQVzFlUa4rgFV3XOhVzBCvRA/exec"; 
        }
        static void Main(string[] args)
        {
            Credential(); 
            GoogleDriveWebRequester.Instance.ReadGoogleSpreadSheet("1BXya0YQq980kbNBN_-hQAvmBrNkHFIoqXJkQTXIsXHQ", (err) => { }, (callback, json) => { 
                HamsterGoogleSheet.DataParser.ParseSheet(json, true, true, new FileWriter()); 
                Console.WriteLine(json);
            });
        }
    }
}
