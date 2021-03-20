using System.Collections.Generic;
using System.Diagnostics;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(List<char>), speractors: new string[] { "list<char>", "List<Char>" })]
    public class CharListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        { 
            var list = new System.Collections.Generic.List<char>();
            if (value == "[]") return list;

            var datas = ReadUtil.GetBracketValueToArray(value);
            if (datas != null)
            {
                foreach (var data in datas)
                    list.Add(char.Parse(data));
            }
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<char>;
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}