namespace GoogleSheet.Type
{
    [Type(typeof(string), new string[] { "string", "String" })]
    public class StringType : IType
    {
        public object DefaultValue => string.Empty;
        public object Read(string value)
        {
            return value;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

