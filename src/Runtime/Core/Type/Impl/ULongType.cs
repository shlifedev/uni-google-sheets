namespace GoogleSheet.Type
{
    [Type(Type: typeof(ulong), TypeName: new string[] { "ulong", "ULong" })]
    public class ULongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            ulong @long = 0;
            var b = ulong.TryParse(value, out @long);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

                //return DefaultValue;
            }
            return @long;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
