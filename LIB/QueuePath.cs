using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    [Serializable]
    public class QueuePath
    {
        private string _inbound = String.Empty;
        private string _outbound = String.Empty;
        private string _ack = String.Empty;

        public QueuePath() { }
        public QueuePath(string inbound, string outbound)
        {
            Inbound = inbound;
            Outbound = outbound;
        }
        public QueuePath(string inbound, string outbound, string ack)
        {
            Inbound = inbound;
            Outbound = outbound;
            Ack = ack;
        }

        public string Inbound
        {
            get { return _inbound; }
            set { _inbound = value; }
        }
        public string Outbound
        {
            get { return _outbound; }
            set { _outbound = value; }
        }
        public string Ack
        {
            get { return _ack; }
            set { _ack = value; }
        }
    }

    [Serializable]
    public class QueuePaths : List<QueuePath>
    {
        public QueuePaths() { }
        public QueuePaths(QueuePaths col)
        {
            this.AddRange(col);
        }
    }
}
