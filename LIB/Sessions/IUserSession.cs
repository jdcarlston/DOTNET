using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB.Data
{
    public interface IUserSession
    {
        [XmlAttribute]
        string SessionId { get; set; }

        [XmlArray("IpAddresses")]
        [XmlArrayItem("IpAddress")]
        List<string> IpAddresses { get; set; }
        string Agent { get; set; }
        string Browser { get; set; }

        UserEvents UserEvents { get; set; }
    }
}
