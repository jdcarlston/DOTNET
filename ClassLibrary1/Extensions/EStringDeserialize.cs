using System.Xml;
using System.Xml.Serialization;

namespace LIB.Extensions
{
    public static class EStringDeserialization
    {
        public static T Deserialize<T>(string xmlString) where T : new()
        {
            return Deserialize<T>(xmlString, null);
        }

        public static T Deserialize<T>(string xmlString, XmlAttributeOverrides overrides) where T : new()
        {
            XmlDocument doc = new XmlDocument();

            if (!xmlString.IsNullOrEmpty())
            {
                doc.LoadXml(xmlString);
                return doc.Deserialize<T>(overrides);
            }

            return default(T);
        }

        public static T DeserializeXmlPath<T>(string path) where T : new()
        {
            return DeserializeXmlPath<T>(path, null);
        }

        public static T DeserializeXmlPath<T>(string path, XmlAttributeOverrides overrides) where T : new()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            return doc.Deserialize<T>(overrides);
        }

        public static T Deserialize<T>(XmlReader r) where T : new()
        {
            return Deserialize<T>(r, null);
        }
        public static T Deserialize<T>(XmlReader r, XmlAttributeOverrides overrides) where T : new()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(r);

            return doc.Deserialize<T>(overrides);
        }
    }
}
