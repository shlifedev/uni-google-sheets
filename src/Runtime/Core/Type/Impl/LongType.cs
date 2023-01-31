namespace GoogleSheet.Type
{
    [Type(Type: typeof(long), TypeName: new string[] { "long", "Long" })]
    public class LongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            long @long = 0;
            var b = long.TryParse(value, out @long);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return @long;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
