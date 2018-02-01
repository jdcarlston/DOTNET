using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using LIB.Data;

namespace LIB
{
    [Serializable]
    public class Test : ModelObject
    {
        private DateTime _timestamp = DateTime.Now;
        private string _description = String.Empty;

        public Test() { }
        public Test(string description)
        {
            _description = description;
        }
        public Test(string description, DateTime timestamp)
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
    [XmlType(TypeName = "Tests")]
    public class Tests : List<Test>
    {
        public Tests() { }
        public Tests(Tests col)
        {
            this.AddRange(col);
        }

        public void Add(string e)
        {
            this.Add(new Test(e));
        }

        public void Add(string e, int id, string sessionid)
        {
            Test obj = new Test(e);
            this.Add(obj);
            //obj.Log(id, sessionid);
        }
    }
}
