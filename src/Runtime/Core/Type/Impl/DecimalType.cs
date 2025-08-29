namespace GoogleSheet.Type
{

    [Type(Type: typeof(decimal), TypeName: new string[] { "decimal", "Decimal" })]
    public class DecimalType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + GetType().Name);

            decimal @decimal = 0;
            var b = decimal.TryParse(value, out @decimal);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            }
            return @decimal;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
