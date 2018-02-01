using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DOTNET
{
    public static class EStringDeserialization
    {
        public static T Deserialize<T>(this string xmlString)
        {
            return Deserialize<T>(xmlString, null);
        }

        public static T Deserialize<T>(this string xmlString, XmlAttributeOverrides overrides)
        {
            XmlDocument doc = new XmlDocument();

            if (xmlString != null && xmlString.Length > 0)
            {
                doc.LoadXml(xmlString);
                return doc.Deserialize<T>(overrides);
            }

            return default(T);
        }

        public static T DeserializeFromFile<T>(this string path)
        {
            return DeserializeFromFile<T>(path, null);
        }

        public static T DeserializeFromFile<T>(this string path, XmlAttributeOverrides overrides)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            return doc.Deserialize<T>(overrides);
        }
    }

}
