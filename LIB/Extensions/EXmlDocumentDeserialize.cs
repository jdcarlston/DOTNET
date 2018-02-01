using System.Xml;
using System.Xml.Serialization;

namespace LIB.Extensions
{
    public static class EXmlDocumentDeserialization
    {
        public static T Deserialize<T>(this XmlDocument doc)
        {
            return Deserialize<T>(doc, null);
        }
        public static T Deserialize<T>(this XmlDocument doc, XmlAttributeOverrides overrides)
        {
            if (doc != null)
            {
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer s = new XmlSerializer(typeof(T), overrides);
                return (T)s.Deserialize(reader);
            }

            return default(T);
        }
    }
}
