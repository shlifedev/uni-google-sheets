using System.Collections.Generic;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(UnityEngine.Vector3), speractors: new string[] { "Vector3" , "vector3" })]
    public class Vector3Type : IType
    {
        public object DefaultValue => 0;
        public object Read(string value)
        { 
            var datas = ReadUtil.GetBracketValueToArray(value);
            return new UnityEngine.Vector3(int.Parse(datas[0]), int.Parse(datas[1]), int.Parse(datas[2])); 
        }

        public string Write(object value)
        {
            return value.ToString();
        }
    }
}
