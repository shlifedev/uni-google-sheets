using GoogleSheet.Type;
using UnityEngine;

namespace UGS.Type
{
    [Type(typeof(Quaternion))]
    public class QuaternionType : IType
    {
        public object DefaultValue => Quaternion.identity;

        /// <summary>
        /// value = google sheet data value. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Read(string value)
        {
            string[] split = value.Split(',');
            float x = float.Parse(split[0]);
            float y = float.Parse(split[1]);
            float z = float.Parse(split[2]);
            float w = float.Parse(split[3]);
            return new UnityEngine.Quaternion(x, y, z, w);
        }


        public string Write(object value)
        {
            Quaternion data = (Quaternion)value;
            return $"{data.x},{data.y}.{data.z},{data.w}";
        }
    }
}
