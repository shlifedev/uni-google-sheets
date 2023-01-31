namespace GoogleSheet.Type
{
    [Type(Type: typeof(byte), TypeName: new string[] { "byte", "Byte" })]
    public class ByteType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            byte @byte = 0;
            var b = byte.TryParse(value, out @byte);


            if (b == false)
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            return @byte;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
