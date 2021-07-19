using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ZG.Core.Type;

namespace Hamster.ZG.Type
{
    
    public static class TypeMap
    {
        public static bool init = false;
        static Dictionary<System.Type, IType> map = new Dictionary<System.Type, IType>();
        static Dictionary<string, System.Type> strTypeMap = new Dictionary<string, System.Type>();
        static Dictionary<string, EnumType> enumMap = new Dictionary<string, EnumType>();
        public static Dictionary<System.Type, IType> Map
        {
            get => map;
        }
        public static Dictionary<string, System.Type> StrMap
        {
            get => strTypeMap;
        }
        public static Dictionary<string, EnumType> EnumMap
        {
            get => enumMap;
        }
        public static void Init()
        {
 
            if (init == false)
            { 
                var subClassesEnum = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(System.Enum));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var value in subClassesEnum)
                {
                    var att = value.GetCustomAttribute(typeof(UGSAttribute));
                    if (att != null)
                    {
                        enumMap.Add(value.Name , new EnumType() { 
                            Assembly = value.Assembly,
                            EnumName = value.Name,
                            NameSpace = value.Namespace ,
                            Type = value
                        });  
                    }
                }
                sw.Stop();
               // UnityEngine.Debug.Log("enum search  " + sw.ElapsedMilliseconds.ToString());
                sw.Reset();
 

                sw.Start();
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
                sw.Stop();
                // UnityEngine.Debug.Log("type map add " + sw.ElapsedMilliseconds.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                init = true;
            }
        }
    }
}
