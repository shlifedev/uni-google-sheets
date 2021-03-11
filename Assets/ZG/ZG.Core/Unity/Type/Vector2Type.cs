using UnityEngine;

namespace Hamster.ZG.Type
{
    [Type(type: typeof(UnityEngine.Vector2), speractors: new string[] { "Vector2" })]
    public class Vector2Type : IType
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
            return new UnityEngine.Vector3(x, y);
        }


        public string Write(object value)
        {
            Vector2 data = (Vector2)value;
            return $"{data.x},{data.y}";
        }
    }
}
