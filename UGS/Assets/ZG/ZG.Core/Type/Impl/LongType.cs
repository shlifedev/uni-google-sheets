namespace Hamster.ZG.Type
{
    [Type(type : typeof(long), speractors : new string[] {"long","Long"})]
    public class LongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            long @long = 0;
            var b = long.TryParse(value, out @long);
            if (b == false)
            {
                return DefaultValue;
            }
            return @long;
        }

        public string Write(object value)
        {
           return value.ToString();
        }
    }
}
