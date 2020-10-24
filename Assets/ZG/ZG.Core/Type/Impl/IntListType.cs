using System.Collections.Generic;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(List<int>), speractors: new string[] { "list<int>", "List<int>" })]
    public class IntListType : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            var list = new System.Collections.Generic.List<int>();
            foreach (var data in datas)
                list.Add(int.Parse(data));
            return list;
        }

        public string Write(object value)
        {
            var list = value as List<int>;  
            return WriteUtil.SetValueToBracketArray(list);
        }
    }
}
