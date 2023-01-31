namespace GoogleSheet.Type
{
    [Type(Type: typeof(uint), TypeName: new[] { "uint", "UInt" })]
    public class UIntType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            uint @int = 0;
            var b = uint.TryParse(value, out @int);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

                //return DefaultValue;
            }
            return @int;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
