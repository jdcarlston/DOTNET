using System;
using System.Text.RegularExpressions;

namespace LIB.Extensions
{
    public static class EDecimalFormat
    {
        public static string FormatPercentage(this decimal amt)
        {
            return Regex.Replace(String.Format("{0:0.00}", amt), @"\.00", "") + "%";
        }
        public static string FormatDailyPercentage(this decimal amt)
        {
            string str = String.Format("{0:0.0000000}", (decimal)(amt / 365));
            return Regex.Replace(str, @"(\d+\.\d{5})\d+", "$1%");
        }
        public static string FormatFee(this decimal amt)
        {
            return Regex.Replace(String.Format("${0:0.00}", amt), @"\.00", "");
        }
        public static string FormatWholeDollar(this decimal amt)
        {
            return Regex.Replace(String.Format("{0:C0}", amt), @"\.00", "");
        }
        public static string FormatDecimal(this decimal amt)
        {
            return Regex.Replace(String.Format("{0:0.00}", amt), @"\.00", "");
        }
    }
}
