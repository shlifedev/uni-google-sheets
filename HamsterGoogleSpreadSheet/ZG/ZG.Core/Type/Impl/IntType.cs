using System.Collections.Generic;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(int), speractors: new string[] { "int", "Int", "Int32" })]
    public class IntType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            int @int = 0;
            var b = int.TryParse(value, out @int);
            if (b == false)
            {
                return DefaultValue;
            }
            return @int;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
