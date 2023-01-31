using System;
using System.Text;

namespace GoogleSheet.Type
{
    [Type(Type: typeof(byte[]), TypeName: new string[] { "byte[]", "Byte[]" })]
    public class ByteArrayType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);

            byte[] bytes = Encoding.Default.GetBytes(value);
            return bytes;
        }

        public string Write(object value)
        {
            return Encoding.Default.GetString(value as byte[] ?? Array.Empty<byte>());
        }
    }
}
