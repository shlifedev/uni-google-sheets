namespace Hamster.ZG.Type
{
    [Type(typeof(float), new string[] { "float", "Float" })]
    public class FloatType : IType
    {  
        public object DefaultValue => 0.0f;
        public object Read(string value)
        {
            float f = 0.0f;
            var b = float.TryParse(value, out f);
            if (b == false)
            {
                return DefaultValue;
            }
            return f; 
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
