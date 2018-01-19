using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Xml.Serialization;
using DOTNET;

namespace DOTNET.Data
{
    [Serializable]
    [XmlType(TypeName = "UserSession")]
    public class UserSession : IUserSession
    {
        private string _sessionid = String.Empty;
        private List<string> _ipaddresses = new List<string>();
        private string _agent = String.Empty;
        private string _browser = String.Empty;
        private string _partnerurlid = string.Empty;

        private UserEvents _userevents = new UserEvents();
        private MsmqConnection _msmqconnection = new MsmqConnection();

        private int _certificateid = 0;

        public UserSession() { }
        public UserSession(UserSession obj) 
        {
            SessionId = obj.SessionId;
            IpAddresses = obj.IpAddresses;
            Agent = obj.Agent;
            Browser = obj.Browser;
            UserEvents = obj.UserEvents;
            MsmqConnection = obj.MsmqConnection;
            CertificateId = obj.CertificateId;
        }

        [XmlAttribute]
        public string SessionId
        {
            get { return _sessionid == null ? String.Empty : _sessionid.Trim(); }
            set { _sessionid = value; }
        }

        [XmlArray("IpAddresses")]
        [XmlArrayItem("IpAddress")]
        public List<string> IpAddresses
        {
            get { return _ipaddresses; }
            set { _ipaddresses = value; }
        }
        public string Agent
        {
            get { return _agent == null ? String.Empty : _agent.Trim(); }
            set { _agent = value; }
        }
        public string Browser
        {
            get { return _browser == null ? String.Empty : _browser.Trim(); }
            set { _browser = value; }
        }

        public string PartnerUrlId { get { return _partnerurlid == null ? String.Empty : _partnerurlid.Trim().Clean(); } set { _partnerurlid = value.Clean(); } }

        [XmlIgnore]
        public UserEvents UserEvents { get { return _userevents; } set { _userevents = value; } }

        public MsmqConnection MsmqConnection { get { return _msmqconnection; } set { _msmqconnection = value; } }

        public int CertificateId { get { return _certificateid; } set { _certificateid = value; } }
    }
}
