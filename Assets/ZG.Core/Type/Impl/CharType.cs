using System;
namespace Hamster.ZG.Type
{
    [Type(typeof(char), new string[] { "char", "Char" })]
    public class CharType : IType
    {
        public object DefaultValue => Char.MinValue;
        public object Read(string value)
        {
            char @char = Char.MinValue;
            var b = Char.TryParse(value, out @char);
            if (b == false)
            {
                return DefaultValue;
            }
            return @char;
        }

        public string Write(object value)
        {
             return value.ToString();
        }
    }
}

