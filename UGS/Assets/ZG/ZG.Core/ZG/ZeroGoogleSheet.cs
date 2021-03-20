 
using Hamster.ZG.IO;
using Hamster.ZG.Type;
using System; 
namespace Hamster.ZG
{
    public static class ZeroGoogleSheet
    {
        private static IParser _dataParser;
        private static IFileReader _dataReader;
        public static IParser DataParser { get => _dataParser; } 
        public static IFileReader DataReader { get => _dataReader;}

        public static void Init(IParser parser, IFileReader reader)
        { 
            TypeMap.Init();  
            _dataParser = parser;
            _dataReader = reader;
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
