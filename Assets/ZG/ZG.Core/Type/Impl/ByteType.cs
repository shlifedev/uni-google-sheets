namespace Hamster.ZG.Type
{
    [Type(type : typeof(byte), speractors : new string[] { "byte", "Byte"})]
    public class ByteType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {
            byte @byte = 0;
            var b = byte.TryParse(value, out @byte);
            if (b == false)
            {
                return DefaultValue;
            }
            return @byte;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
