namespace Hamster.ZG.Type
{
    [Type(type : typeof(short), speractors : new string[] { "short", "Short"})]
    public class ShortType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            short @short = 0;
            var b = short.TryParse(value, out @short);
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
