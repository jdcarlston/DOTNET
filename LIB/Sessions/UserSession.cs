using LIB.Extensions;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB.Data
{
    [Serializable]
    [XmlType(TypeName = "UserSession")]
    public class UserSession : ModelObject, IUserSession
    {
        private string _sessionid = String.Empty;
        private List<string> _ipaddresses = new List<string>();
        private string _agent = String.Empty;
        private string _browser = String.Empty;

        private QueuePath _queueconnection = new QueuePath();
        private UserEvents _userevents = new UserEvents();

        public UserSession() { }
        public UserSession(UserSession obj) 
        {
            SessionId = obj.SessionId;
            IpAddresses = obj.IpAddresses;
            Agent = obj.Agent;
            Browser = obj.Browser;
            UserEvents = obj.UserEvents;
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

        public QueuePath QueuePath { get { return _queueconnection; } set { _queueconnection = value; } }

        [XmlIgnore]
        public UserEvents UserEvents { get { return _userevents; } set { _userevents = value; } }
    }
}
