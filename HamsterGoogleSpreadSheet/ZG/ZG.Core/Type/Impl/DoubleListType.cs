using System.Collections.Generic;
using System.Diagnostics;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(List<double>), speractors: new string[] { "list<double>", "List<Double>" })]
    public class DoubleListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        { 
            var list = new System.Collections.Generic.List<double>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(double.Parse(data));
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