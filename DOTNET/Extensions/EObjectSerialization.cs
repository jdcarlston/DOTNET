using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DOTNET
{
    public static class EObjectSerialization
    {
        public static string Serialize<T>(this Object obj)
        {
            return Serialize<T>(obj, null);
        }
        public static string Serialize<T>(this Object obj, XmlAttributeOverrides overrides)
        {
            XmlSerializer s = new XmlSerializer(typeof(T), overrides);

            StringBuilder sb = new StringBuilder();
            TextWriter textWriter = new EncodedStringWriter(sb, Encoding.UTF8);

            if (obj != null)
                s.Serialize(textWriter, obj);

            return textWriter.ToString();
        }
    }
}
