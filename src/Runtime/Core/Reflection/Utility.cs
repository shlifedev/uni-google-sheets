using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleSheet.Reflection
{
    public class Utility
    {
        /// <summary>
        /// Get all Subclass Of ~~
        /// </summary>
        /// <param name="parent"></param> 
        /// <returns></returns>
        public static IEnumerable<System.Type> GetAllSubclassOf(System.Type parent)
        {
            var type = parent;
            var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));
            return types;
        }


        public static string TypeNameWithNamespace(System.Type type)
        {
            if (string.IsNullOrEmpty(type.Namespace))
                return type.Name;
            else
                return $"{type.Namespace}.{type.Name}";
        }
    }
}
