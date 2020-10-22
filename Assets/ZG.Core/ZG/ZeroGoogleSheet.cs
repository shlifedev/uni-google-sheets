using Hamster.ZG.Http;
using Hamster.ZG.Type;
using System; 
using System.Diagnostics;
namespace Hamster.ZG
{
    public static class ZeroGoogleSheet
    { 
        static IParser dataReader; 
        public static IParser DataReader { get => dataReader; } 
        public static void Init(IParser reader)
        { 
            TypeMap.Init();  
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
