namespace Hamster.ZG.Type
{
     
    [Type(type : typeof(decimal), speractors : new string[] { "decimal", "Decimal"})]
    public class DecimalType : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        {

            decimal @decimal = 0;
            var b = decimal.TryParse(value, out @decimal);
            if (b == false)
            {
                return DefaultValue;
            }
            return @decimal;
        }

        public string Write(object value)
        {
           return value.ToString();
        }
    }
}
