using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RomanNumeralTester
{
    [TestClass]
    public class RomanNumeralToDigitConversionTest
    {
        [TestMethod]
        [DataRow("0", "")]
        [DataRow("1", "I")]
        [DataRow("2", "II")]
        [DataRow("3", "III")]
        [DataRow("4", "IV")]
        [DataRow("5", "V")]
        [DataRow("6", "VI")]
        [DataRow("9", "IX")]
        [DataRow("10", "X")]
        [DataRow("40", "XL")]
        [DataRow("50", "L")]
        [DataRow("100", "C")]
        [DataRow("100", "C")]
        [DataRow("500", "D")]
        [DataRow("1000", "M")]
        public void TestArabicToRomanConvert(int arabic, string roman)
        {
            Assert.AreEqual(ArabicToRoman.Convert(arabic), roman);
        }
    }

    public static class ArabicToRoman
    {
        static List<KeyValuePair<int, string>> RomanNumeralMap = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(1000, "M"),
            new KeyValuePair<int, string>(500, "D"),
            new KeyValuePair<int, string>(100, "C"),
            new KeyValuePair<int, string>(50, "L"),
            new KeyValuePair<int, string>(10, "X"),
            new KeyValuePair<int, string>(5, "V"),
            new KeyValuePair<int, string>(1, "I")
        };
    

        public static string Convert(this int arabic)
        {
            string roman = String.Empty;

            var mapping = RomanNumeralMap.Find(m => m.Key <= arabic);
            roman = mapping.Key + Convert(arabic = mapping.Key);

            return roman;
        }
    }

}
