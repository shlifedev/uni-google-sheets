namespace GoogleSheet.Type
{
    [Type(type: typeof(ushort), speractors: new string[] { "ushort" })]
    public class UShortType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            ushort @short = 0;
            var b = ushort.TryParse(value, out @short);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return @short;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
