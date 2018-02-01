using System;

namespace LIB.Extensions
{
    public static class EDateTime
    {
        public static int AgeYears(this DateTime date)
        {
            int age = DateTime.Now.Year - date.Year;
            if (DateTime.Now < date.AddYears(age))
                age--;
            return age;
        }
        
        public static bool Is18Years(this DateTime date)
        {
            return AgeYears(date) >= 18 ? true : false;
        }

        public static bool Is21Years(this DateTime date)
        {
            return AgeYears(date) >= 21 ? true : false;
        }

        public static string ToSqlDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd hh:mm:ss.fff");
        }

        internal static DateTime GetDate(string year, string month, string day)
        {
            Int32 y = 0;
            Int32 m = 0;
            Int32 d = 0;

            try { y = Convert.ToInt32(year); }
            catch { throw; }

            try { m = Convert.ToInt32(month); }
            catch { throw; }

            try { d = Convert.ToInt32(day); }
            catch { throw; }

            return new DateTime(y, m, d);
        }
    }
}
