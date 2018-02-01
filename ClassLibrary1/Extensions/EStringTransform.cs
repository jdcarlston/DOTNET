using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LIB.Extensions
{
    public static class EStringTransform
    {
        //Clean String Input
        public static string Clean(this string value)
        {
            if (value.IsNullOrEmpty())
                return String.Empty;

            //Remove and replace basic characters that are not allowed
            value = Regex.Replace(value, @"[ñ]", "n", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            value = Regex.Replace(value, @"[é]", "e", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            value = Regex.Replace(value, @"[á]", "a", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //value = Regex.Replace(value, @"/^\s\s*/", "", RegexOptions.Singleline);
            //value = Regex.Replace(value, @"\s\s*$", "", RegexOptions.Singleline);
            value = Regex.Replace(value, @"[^a-z0-9\d\-\=\!\@\#\$\%\^\&\*\(\)_\+\[\]\\\{\}\'\"";:\<\>\?,\.\\/\s ]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            return value.ToUpper();
        }

        public static string CleanNumeric(this string value)
        {
            return (!value.IsNullOrEmpty()) ? Regex.Replace(value, @"[^0123456789]", "") : String.Empty;
        }
        public static string CleanAlpha(this string value)
        {
            return (!value.IsNullOrEmpty()) ? Regex.Replace(value, @"[^a-zA-Z]", "") : String.Empty;
        }

        public static string CleanAlphaNumeric(this string value)
        {
            return (!value.IsNullOrEmpty()) ? Regex.Replace(value, @"[^a-zA-Z0123456789]", "") : String.Empty;
        }

        public static string CleanName(this string value)
        {
            return (!value.IsNullOrEmpty()) ? Regex.Replace(value, @"[^a-zA-Z\s\-',]", "") : String.Empty;
        }

        public static string HtmlEncode(this string value)
        {
            //Make everyting uppercase and HTML Encode 
            return HttpContext.Current.Server.HtmlEncode(value);
        }

        public static string HtmlDecode(this string value)
        {
            return HttpContext.Current.Server.HtmlDecode(value);
        }


        //Mask Strings
        public static string Mask(this string value)
        {
            if (!value.IsNullOrEmpty())
                return (value.Length.Equals(16)) ? Mask(value, true) : Mask(value, false);
            return value;
        }
        public static string Mask(this string value, bool isPAN)
        {
            StringBuilder sb = new StringBuilder();
            if (isPAN && value.Length.Equals(16))
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
            if (!value.IsNullOrEmpty())
                return TextInfo.ToTitleCase(value);
            return value;
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static string Left(this string value, int length)
        {
            if (!value.IsNullOrEmpty())
                return value.Substring(0, Math.Min(length, value.Length));
            throw new Exception("String was null or empty trying to parse Left.");
        }
        public static string Right(this string value, int length)
        {
            if (!value.IsNullOrEmpty())
                return value.Substring(value.Length - length, length);
            throw new Exception("String was null or empty trying to parse Right.");
        }

    }
}
