using System;
using System.Xml.Serialization;

namespace LIB.Q.Monitor
{
    [Serializable]
    public class QueueRequestCall
    {
        private string _requesturl = string.Empty;
        private string _soapfile = string.Empty;
        //private string _soapmethod = string.Empty;
        private string _soapaction = string.Empty;
        private string _namespace = string.Empty;
        private string _importingnode = string.Empty;
        private string _host = "localhost";
        private int _timeout = 90000; //in milliseconds 60000 ms = 1 minute

        public string RequestUrl
        {
            get { return _requesturl; }
            set { _requesturl = value; }
        }
        public string SoapAction
        {
            get { return _soapaction; }
            set { _soapaction = value; }
        }

        public string SoapFile
        {
            get { return _soapfile; }
            set { _soapfile = value; }
        }
        public string NameSpace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
        public string ImportingNode
        {
            get { return _importingnode; }
            set { _importingnode = value; }
        }

        [XmlAttribute]
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        [XmlAttribute]
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
    }
}
