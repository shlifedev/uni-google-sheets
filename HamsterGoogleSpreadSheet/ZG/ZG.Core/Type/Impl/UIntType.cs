namespace Hamster.ZG.Type
{
    [Type(type : typeof(uint), speractors : new[] { "uint", "UInt"})]
    public class UIntType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            uint @int = 0;
            var b = uint.TryParse(value, out @int);
            if (b == false)
            {
                return DefaultValue;
            }
            return @int;
        }


        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
