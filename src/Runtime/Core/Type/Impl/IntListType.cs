using System.Collections.Generic;

namespace GoogleSheet.Type
{
    [Type(Type: typeof(List<int>), TypeName: new string[] { "list<int>", "List<int>" })]
    public class IntListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {

            if (string.IsNullOrEmpty(value))
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);


            var list = new System.Collections.Generic.List<int>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(int.Parse(data));
            }
            else
            {
                throw new UGSValueParseException("Parse Faield => " + value + " To " + this.GetType().Name);
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<int>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}
