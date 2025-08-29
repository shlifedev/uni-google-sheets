using System;
namespace GoogleSheet.Type
{
    [Type(typeof(char), new string[] { "char", "Char" })]
    public class CharType : IType
    {
        public object DefaultValue => Char.MinValue;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + GetType().Name);

            char @char = Char.MinValue;
            var b = Char.TryParse(value, out @char);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            }
            return @char;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

