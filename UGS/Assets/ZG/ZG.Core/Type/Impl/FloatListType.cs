using System.Collections.Generic;
using System.Diagnostics;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(List<float>), speractors: new string[] { "list<float>", "List<Float>" })]
    public class FloatListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        { 
            var list = new System.Collections.Generic.List<float>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(float.Parse(data));
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<float>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}