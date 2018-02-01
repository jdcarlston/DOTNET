using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DOTNET;

namespace DOTNET.Data
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

        string PartnerUrlId { get; set; }

        UserEvents UserEvents { get; set; }

        int CertificateId { get; set; }
    }
}
