using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LIB
{
    public static class StringCheck
    {
        public static bool IsValidSolicitation(string value)
        {
            return Regex.IsMatch(value, @"[0-9]{4}[0-9A-Z]{8}");
        }
        //public static bool IsValidEmail(string value)
        //{
        //    return Regex.IsMatch(value, @"");
        //}
        public static bool IsValidFirstName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z\s-\']{0,13}[a-zA-Z]{0,1}$");
        }
        public static bool IsValidMiddleName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z]{0,1}$");
        }
        public static bool IsValidLastName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z\s-\']{0,24}[a-zA-Z]{0,1}$");
        }
        public static bool IsValidSsn(string value)
        {
            return Regex.IsMatch(value, @"^((?!000)(?:[0-6][0-9]{2}|7[0-2][0-9]|73[0-3]|7[5-6][0-9]|77[0-2]))-?((?!00)[0-9]{2})-?((?!0000)[0-9]{4})$");
            //return Regex.IsMatch(value, @"^[0-9]{3]-?[0-9]{2]-?[0-9]{4]$");
        }
        public static bool IsValidMothersMaidenName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z]+(([\'\,\.\-\s][a-zA-Z])?[a-zA-Z]*)*$");
        }
        public static bool IsValidPhone(string value)
        {
            return Regex.IsMatch(value, @"^$|^(\(?[2-9][0-9]{2}\)?[\s]?|[2-9][0-9]{2}[\-\s\.]?)[2-9][0-9]{2}[\-\s\.]?[0-9]{4}$");
        }
        public static bool IsValidStreetNumber(string value)
        {
            return Regex.IsMatch(value, @"^[0-9]{1,10}$");
        }
        public static bool IsValidRuralRoute(string value)
        {
            return Regex.IsMatch(value, @"^[0-9a-zA-Z][^\-\!\@\$\&\*\(\)_\'\;\:\?\,\\\/]{0,15}$");
        }
        public static bool IsValidBoxNumber(string value)
        {
            return Regex.IsMatch(value, @"^[0-9][^\!\@\$\&\*\(\)_\'\;\:\?\,\\\/\s]{0,9}$");
        }
        public static bool IsValidStreetName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z0-9][^\-\!\@\$\&\*\(\)_\'\;\:\?\,\\\/]{1,29}$");
        }
        public static bool IsValidUnit(string value)
        {
            return Regex.IsMatch(value, @"^$|^[^\-\!\@\$\&\*\(\)_\'\;\:\?\,\\\/]{1,10}$");
        }
        public static bool IsValidCity(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z0-9][^\-\!\@\$\&\*\(\)_\'\;\:\?\,\\\/]{1,29}$");
        }
        public static bool IsValidState(string value)
        {
            return Regex.IsMatch(value, @"^A[AEKLPRSZ]|C[AOT]|D[CE]|FL|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEINOPST]|N[CDEHJMVY]|O[HKR]|P[AR]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY]$");
        }
        public static bool IsValidZip(string value)
        {
            return Regex.IsMatch(value, @"^(?!0{5})(\d{5})(?!-?0{4})(|-?\d{4})?$");
            //return Regex.IsMatch(value, @"^\d{5}$|^\d{5}-?\d{4}$");
        }
        public static bool IsValidRoutingNumber(string value)
        {
            bool test = Regex.IsMatch(value, @"^[0-9]{9}$");
            return test;
        }
        public static bool IsValidBankAccount(string value)
        {
            bool test = Regex.IsMatch(value, @"^[0-9]{1,17}$");
            return test;
        }
    }
}
