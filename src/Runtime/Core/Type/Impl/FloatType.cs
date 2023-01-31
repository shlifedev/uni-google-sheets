namespace GoogleSheet.Type
{
    [Type(typeof(float), new string[] { "float", "Float" })]
    public class FloatType : IType
    {
        public object DefaultValue => 0.0f;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            float f = 0.0f;
            var b = float.TryParse(value, out f);
            if (b == false)
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

                //return DefaultValue;
            }
            return f;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
