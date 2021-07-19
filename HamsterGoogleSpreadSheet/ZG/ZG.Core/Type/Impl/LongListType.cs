using System.Collections.Generic;
using System.Diagnostics;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(List<long>), speractors: new string[] { "list<long>", "List<Long>" })]
    public class LongListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        { 
            var list = new System.Collections.Generic.List<long>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(long.Parse(data));
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<long>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}