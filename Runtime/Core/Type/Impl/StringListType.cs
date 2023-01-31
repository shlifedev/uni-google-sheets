namespace GoogleSheet.Type
{
    [Type(typeof(System.Collections.Generic.List<string>), new string[] { "list<string>", "List<string>" })]
    public class StringListType : IType
    {
        public object DefaultValue => string.Empty;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            var list = new System.Collections.Generic.List<string>();
            foreach (var data in datas)
            {
                list.Add(data);
            }
            return list;

        }

        public string Write(object value)
        {
            return WriteUtil.SetValueToBracketArray<System.Collections.Generic.List<string>, string>(value);
        }
    }
}

