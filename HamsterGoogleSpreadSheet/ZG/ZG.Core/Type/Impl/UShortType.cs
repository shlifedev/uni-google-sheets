namespace Hamster.ZG.Type
{
    [Type(type : typeof(ushort), speractors : new string[] {"ushort"})]
    public class UShortType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            ushort @short = 0;
            var b = ushort.TryParse(value, out @short);
            if (b == false)
            {
                return DefaultValue;
            }
            return @short;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
