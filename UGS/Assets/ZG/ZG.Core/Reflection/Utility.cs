using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Reflection
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
    }
}
