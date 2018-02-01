using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
