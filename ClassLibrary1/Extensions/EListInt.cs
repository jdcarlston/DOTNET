using System.Collections.Generic;

namespace LIB.Extensions
{
    public static class EListIntConversion
    {
        public static string ToCsvString(this List<int> value)
        {
            return string.Join<int>(",", value);
        }
    }
}
