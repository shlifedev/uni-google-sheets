using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Hamster.ZG.Type
{
    public static class TypeMap
    {
        public static bool init = false;
        static Dictionary<System.Type, IType> map = new Dictionary<System.Type, IType>();
        static Dictionary<string, System.Type> strTypeMap = new Dictionary<string, System.Type>();
        public static Dictionary<System.Type, IType> Map
        {
            get => map;
        }
        public static Dictionary<string, System.Type> StrMap
        {
            get => strTypeMap;
        }
        public static void Init()
        {
            if (init == false)
            {
                var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(IType));
                foreach (var data in subClasses)
                {
                    if (data.IsInterface)
                        continue;
                    var instance = Activator.CreateInstance(data);
                    var att = instance.GetType().GetCustomAttribute<TypeAttribute>();
                    if (att != null)
                    {
#if !UNITY_EDITOR
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("[TypeMap] Added " + att.type.ToString() + "  " + instance.ToString());
#endif
                        if (!Map.ContainsKey(att.type))
                        {
                            Map.Add(att.type, (IType)instance);
                        }
                        foreach (var sepractor in att.sepractors)
                        {
                            if (StrMap.ContainsKey(sepractor) == false)
                                StrMap.Add(sepractor, att.type);
#if !UNITY_EDITOR
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(" ㄴ[TypeMap] Added Sepractors " + sepractor);
#endif
                        }
                    }
                    else
                    {
#if !UNITY_EDITOR
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[TypeMap] Require Set Type Attribute => " + instance.ToString());
#endif
                        throw new Hamster.ZG.Exception.RequireTypeAttributeException();
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                init = true;
            }
        }
    }
}
