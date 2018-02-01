using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public static class Age
    {
        public static int AgeYears(DateTime date)
        {
            int age = DateTime.Now.Year - date.Year;
            if (DateTime.Now < date.AddYears(age))
                age--;
            return age;
        }

        public static bool Is16Years(DateTime date)
        {
            return AgeYears(date) >= 16 ? true : false;
        }

        public static bool Is18Years(DateTime date)
        {
            return AgeYears(date) >= 18 ? true : false;
        }

        public static bool Is21Years(DateTime date)
        {
            return AgeYears(date) >= 21 ? true : false;
        }
    }
}
