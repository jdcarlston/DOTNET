using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LIB
{
    public static class EObjectSerialization
    {
        public static string Serialize<T>(Object obj)
        {
            return Serialize<T>(obj, null);
        }
        public static string Serialize<T>(Object obj, XmlAttributeOverrides overrides)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(T), overrides);

                StringBuilder sb = new StringBuilder();
                TextWriter textWriter = new EncodedStringWriter(sb, Encoding.UTF8);

                if (null != obj)
                    s.Serialize(textWriter, obj);

                return textWriter.ToString();
            }
            catch
            {
                CustomXmlSerializer s = new CustomXmlSerializer();

                StringBuilder sb = new StringBuilder();
                TextWriter textWriter = new EncodedStringWriter(sb, Encoding.UTF8);
                XmlTextWriter xmlWriter = new XmlTextWriter(textWriter);

                if (obj != null)
                    s.WriteXml(obj, xmlWriter);

                return sb.ToString();
            }
        }
        public static string Serialize<T>(this T obj) where T : new()
        {
            return Serialize<T>(obj, null);
        }

        public static string Serialize<T>(this T obj, XmlAttributeOverrides overrides) where T : new()
        {
            //if (typeof(T).GetInterfaces().Contains(typeof(IEnumerable)))
            //    return CustomSerializeObject<T>(obj, null);
            //else
            return SerializeObject<T>(obj, null);
        }

        private static string CustomSerializeObject<T>(T obj, XmlAttributeOverrides overrides)
        {
            CustomXmlSerializer s = new CustomXmlSerializer();

            StringBuilder sb = new StringBuilder();
            TextWriter textWriter = new EncodedStringWriter(sb, Encoding.UTF8);
            XmlTextWriter xmlWriter = new XmlTextWriter(textWriter);

            if (obj != null)
                s.WriteXml(obj, xmlWriter);

            return sb.ToString();
        }

        private static string SerializeObject<T>(T obj, XmlAttributeOverrides overrides) where T : new()
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
