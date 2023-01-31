using GoogleSheet.Type;
using UnityEngine;

namespace UGS.Type
{
    [Type(typeof(UnityEngine.Vector2))]
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
            try
            {
                string[] split = value.Split(',');
                float x = float.Parse(split[0]);
                float y = float.Parse(split[1]);
                var vec2 = new UnityEngine.Vector2(x, y);
                return vec2;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return default;
            }
        }


        public string Write(object value)
        {
            Vector2 data = (Vector2)value;
            return $"{data.x},{data.y}";
        }
    }
}
