using System;
using System.Xml.Serialization;

namespace LIB
{
    public interface IModelObject : IGhostable
    {
        [XmlAttribute]
        int Id { get; set; }
        DateTime TS { get; set; }
    }
}
