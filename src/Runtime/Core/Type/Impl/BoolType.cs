namespace GoogleSheet.Type
{
    [Type(typeof(bool), new string[] { "bool", "Bool" })]
    public class BoolType : IType
    {
        public object DefaultValue => false;
        public object Read(string value)
        {
            return bool.TryParse(value, out var result) && result;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

