

namespace GoogleSheet.Type
{
    [Type(Type: typeof(MyCustomStruct), TypeName: new string[] { "custom", "Custom" })]
    public class MyCustomStructType : IType
    {
        public object DefaultValue => 0;

        /// <summary>
        /// value = google sheet data value. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Read(string value)
        {
            var data = ReadUtil.GetBracketValueToArray(value);
            return new MyCustomStruct(data[0], int.Parse(data[1]), float.Parse(data[2]));
        }


        public string Write(object value)
        {
            MyCustomStruct data = (MyCustomStruct)value;
            return $"({data.value},{data.value2},{data.value3})";
        }
    }
}
