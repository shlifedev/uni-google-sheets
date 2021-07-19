using System;
using System.Text;

namespace Hamster.ZG.Type
{
    [Type(type : typeof(byte[]), speractors : new string[] { "byte[]", "Byte[]"})]
    public class ByteArrayType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if(string.IsNullOrEmpty(value))
                return DefaultValue;
            byte[] bytes = Encoding.Default.GetBytes(value);
            return bytes;
        }

        public string Write(object value)
        {
            return Encoding.Default.GetString(value as byte[] ?? Array.Empty<byte>()); 
        }
    }
}
