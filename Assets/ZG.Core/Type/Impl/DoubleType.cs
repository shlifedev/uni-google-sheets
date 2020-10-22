namespace Hamster.ZG.Type
{
    [Type(type: typeof(double), speractors: new string[] { "double", "Double" })]
    public class DoubleType : IType
    {
        public object DefaultValue => 0.0d;
        public object Read(string value)
        {
            double @double = 0;
            var b = double.TryParse(value, out @double);
            if (b == false)
            {
                return DefaultValue;
            }
            return @double;
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}

