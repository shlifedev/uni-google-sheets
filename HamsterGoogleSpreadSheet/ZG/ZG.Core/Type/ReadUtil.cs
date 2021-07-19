namespace Hamster.ZG.Type
{
    public static class ReadUtil
    {
        public static string[] GetBracketValueToArray(string value)
        {
            if (value.Length == 0) return null;
            if (value[0] == '[' && value[value.Length - 1] == ']')
            {
                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);
                var split = value.Split(',');
                return split;
            }
            return null;
        }
        public static string[] GetParenthesisValueToArray(string value)
        {
            if (value.Length == 0) return null;
            if (value[0] == '(' && value[value.Length - 1] == ')')
            {
                value = value.Remove(0, 1);
                value = value.Remove(value.Length - 1, 1);
                var split = value.Split(',');
                return split;
            }
            return null;
        }
    }
}
