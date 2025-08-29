using System.Collections.Generic;

namespace GoogleSheet.Type
{
    [Type(Type: typeof(List<double>), TypeName: new string[] { "list<double>", "List<Double>" })]
    public class DoubleListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);


            var list = new System.Collections.Generic.List<double>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(double.Parse(data));
            }
            else
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<double>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}