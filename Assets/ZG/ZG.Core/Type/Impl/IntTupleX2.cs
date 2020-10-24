namespace Hamster.ZG.Type
{
    [Type(typeof((int, int)), new string[] { "(int,int)", "(Int32,Int32)" })]
    public class IntTupleX2Type : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            if(datas.Length == 0 || datas.Length == 1 || datas.Length > 2) return DefaultValue;
            else
            { 
                return (datas[0], datas[1]);
            }  
        }

        public string Write(object value)
        {
            var tuple = (((int,int))value); 
            return $"[{tuple.Item1},{tuple.Item2}]";
        }
    }
}
