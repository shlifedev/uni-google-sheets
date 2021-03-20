namespace Hamster.ZG.Type
{
    [Type(type : typeof(ulong), speractors : new string[] {"ulong","ULong"})]
    public class ULongType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            ulong @long = 0;
            var b = ulong.TryParse(value, out @long);
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
