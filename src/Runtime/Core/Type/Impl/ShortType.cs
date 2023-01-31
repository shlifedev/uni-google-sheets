namespace GoogleSheet.Type
{
    [Type(Type: typeof(short), TypeName: new string[] { "short", "Short" })]
    public class ShortType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            short @short = 0;
            var b = short.TryParse(value, out @short);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

                //return DefaultValue;
            }
            return @short;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
