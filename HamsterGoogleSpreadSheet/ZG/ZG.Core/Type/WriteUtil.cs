using System.Collections.Generic;
using System.Text;

namespace Hamster.ZG.Type
{
    public static class WriteUtil
    {
        public static string SetValueToBracketArray<T,G>(object value) where T : List<G>
        {
            return SetValueToBracketArray(value as T);
        }
        public static string SetValueToBracketArray<T>(System.Collections.Generic.List<T> value)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < value.Count; i++)
            {
                string data = value[i].ToString();
                builder.Append(data);
                if (i != value.Count)
                    builder.Append(",");
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
