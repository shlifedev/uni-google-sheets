using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Type
{
    public class EnumType
    {
        public System.Type Type { get; set; }
        public Assembly Assembly { get; set; }
        public string NameSpace { get; set; }
        public string EnumName { get; set; }

        public object Read(string value)
        {
            return System.Enum.Parse(Type, value);
        }
        public string Write(object value)
        {
            return value.ToString();
        } 
    }
}
