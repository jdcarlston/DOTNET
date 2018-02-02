using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LIB.Q.Monitor
{
    public enum MonitoredQueueTypes { DEV, LIVE, UAT }
    [Serializable]
    public class MonitoredQueue
    {
        private int _id = 0;
        private MonitoredQueueTypes _type = MonitoredQueueTypes.DEV;
        private string _path = string.Empty;
        private QueueRequestCall _queuerequestcall = new QueueRequestCall();

        [XmlAttribute]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlAttribute]
        public MonitoredQueueTypes Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public QueueRequestCall QueueRequestCall
        {
            get { return _queuerequestcall; }
            set { _queuerequestcall = value; }
        }
    }

    [Serializable]
    [XmlRoot("MonitoredQueues")]
    public class MonitoredQueues : List<MonitoredQueue>
    { }
}
