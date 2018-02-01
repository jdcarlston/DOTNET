using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB
{
    [Serializable]
    public class UserEvent : ModelObject
    {
        private DateTime _timestamp = DateTime.Now;
        private string _description = String.Empty;

        public UserEvent() { }
        public UserEvent(string description)
        {
            _description = description;
        }
        public UserEvent(string description, DateTime timestamp)
        {
            _timestamp = timestamp;
            _description = description;
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

        public void Add(string e)
        {
            this.Add(new UserEvent(e));
        }

        public void Add(string e, int id, string sessionid)
        {
            UserEvent ue = new UserEvent(e);
            this.Add(ue);
            //ue.Log(id, sessionid);
        }
    }
}
