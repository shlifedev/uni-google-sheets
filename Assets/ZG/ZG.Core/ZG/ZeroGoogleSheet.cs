 
using Hamster.ZG.IO;
using Hamster.ZG.Type;
using System; 
using System.Diagnostics;
namespace Hamster.ZG
{
    public static class ZeroGoogleSheet
    {
        static IParser dataParser;
        static IFileReader dataReader;
        public static IParser DataParser { get => dataParser; } 
        public static IFileReader DataReader { get => dataReader;}

        public static void Init(IParser parser, IFileReader reader)
        { 
            TypeMap.Init();  
            dataParser = parser;
            dataReader = reader;
        }
         
        public static object TestRead(string sepractor, string value)
        {
            var isExistType = TypeMap.StrMap.ContainsKey(sepractor);
            if (isExistType)
            {
                var refType = TypeMap.Map[TypeMap.StrMap[sepractor]];
                var data = refType.Read(value);
                Console.WriteLine("value : " + data);
                
                return data;
            }
            else
            {
                Console.WriteLine("not exist type sepractor => " + sepractor);
            }

            return null;
        }
    }
}
