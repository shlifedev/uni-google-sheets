using UnityEngine;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(UnityEngine.Vector3), speractors: new string[] { "Vector3" })]
    public class Vector3Type : IType
    {
        public object DefaultValue => 0;

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

            return new UnityEngine.Vector3(x, y, z);
        }


        public string Write(object value)
        {
            Vector3 data = (Vector3)value;
            return $"{data.x},{data.y},{data.z}";
        }
    }
}
