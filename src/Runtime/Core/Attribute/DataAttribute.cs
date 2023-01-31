namespace GoogleSheet.Attribute
{
    public class DataAttribute : System.Attribute
    {
        public DataAttribute(System.Type type, object data)
        {
            Type = type;
            Data = data;
        }

        public System.Type Type { get; }
        public object Data { get; }
    }
}
