using System;
using System.Globalization;
using System.Text;

namespace DOTNET
{
    public static class EString
    {
        //Mask Strings
        public static string Mask(this string value)
        {
            return (value.Length.Equals(16)) ? Mask(value, true) : Mask(value, false);
        }
        public static string Mask(this string value, bool iscardnumber)
        {
            StringBuilder sb = new StringBuilder();
            if (iscardnumber && value.Length.Equals(16))
            {
                sb.Append(value.Substring(0, 6));
                sb.Append(new String('*', 6));
                sb.Append(value.Substring(12, 4));

                return sb.ToString();
            }
            else if (value.Length > 4)
            {
                sb.Append(new String('*', value.Length - 4));
                sb.Append(value.Substring(value.Length - 4, 4));

                return sb.ToString();
            }

            return new string('*', value.Length);
        }

        //Make first letters in each word of string uppercase
        public static string ToTitleCase(this string value)
        {
            TextInfo TextInfo = new CultureInfo("en-US", false).TextInfo;
            return TextInfo.ToTitleCase(value);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
    }
}
