namespace GoogleSheet.Type
{
    [Type(Type: typeof(int), TypeName: new string[] { "int", "Int", "Int32" })]
    public class IntType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            int @int = 0;
            var b = int.TryParse(value, out @int);
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
