using System.Collections.Generic;

namespace LIB.Extensions
{
    public static class EStringList
    {
        public static string ToQuoteString(this List<string> value)
        {
            return ToQuoteString(value, '\'');
        }
        public static string ToQuoteString(this List<string> value, char quote)
        {
            if (value.Count > 0)
            {
                string[] arr = value.ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = quote + arr[i].Replace("'", "''") + quote;
                }
                return string.Join(",", arr);
            }
            return string.Empty;
        }
    }
}
