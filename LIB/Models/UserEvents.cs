using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB
{
    [Serializable]
    public class UserEvent : ModelObject
    {
        private string _sessionid = String.Empty;
        private DateTime _timestamp = DateTime.Now;
        private string _description = String.Empty;

        public UserEvent() { }

        public UserEvent(string sessionid, string description)
        {
            _sessionid = sessionid;
            _description = description;
        }
        public UserEvent(int id, string sessionid, string description, DateTime timestamp)
        {
            _sessionid = sessionid;
            _description = description;
            _timestamp = timestamp;
        }

        public string SessionId
        {
            get { return _sessionid; }
            set { _sessionid = value; }
        }
        [XmlAttribute]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        [XmlText]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

    }

    [Serializable]
    [XmlType(TypeName = "UserEvents")]
    public class UserEvents : List<UserEvent>
    {
        public UserEvents() { }
        public UserEvents(UserEvents col)
        {
            this.AddRange(col);
        }
    }
}
