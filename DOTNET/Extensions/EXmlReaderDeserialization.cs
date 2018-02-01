using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DOTNET
{
    public static class XmlReaderDeserializationExtension
    {
        public static T Deserialize<T>(this XmlReader r)
        {
            return Deserialize<T>(r, null);
        }
        public static T Deserialize<T>(this XmlReader r, XmlAttributeOverrides overrides)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(r);

            return doc.Deserialize<T>(overrides);
        }
    }
}
