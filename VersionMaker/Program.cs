using System;
using System.Collections.Generic;

namespace VersionMaker
{
    public class UGSVersion
    {
 

        public string Version;
        public List<string> ChangeLogs = new List<string>();

         
        public void CurrentInfo()
        {
            Console.Clear();
            Console.WriteLine($"Version : {Version}");
            Console.WriteLine($"Change Logs");
            ChangeLogs.ForEach(x => {
                Console.WriteLine($" - : {x}");
            });
             
        }
    }
    public class Program
    {
        static UGSVersion version = new UGSVersion();

        static void Main(string[] args)
        {
            Console.WriteLine("UGS를 릴리즈할 버전을 입력하세요");
            var inputVersion = Console.ReadLine();
            version.Version = inputVersion;
            version.CurrentInfo();
            while (true)
            { 
                Console.WriteLine("체인지 로그를 입력하세요. y 만 입력시 종료, n을 입력시 이전에 입력한 체인지 로그 삭제");

                var inputLog = Console.ReadLine();
                if(inputLog == "n")
                {
                    version.ChangeLogs.RemoveAt(version.ChangeLogs.Count - 1); 
                    version.CurrentInfo();
                    continue;
                }
                if(inputLog == "y")
                {
                    version.CurrentInfo();
                    break;
                }
                version.ChangeLogs.Add(inputLog);
                version.CurrentInfo();
            }
        }

        static void ShowUGSInfo()
        {
 
        }
    }
}
