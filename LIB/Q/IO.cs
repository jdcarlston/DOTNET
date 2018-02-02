using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Messaging;
using System.Text;

namespace LIB.Q
{
    public class IO
    {
        private static int MaxTimeoutSeconds = 360;
        public static bool oldqueue = false;

        public static void InQueueFillRow(Object obj, out SqlString id, out SqlString label, out SqlXml xmlmsg, out SqlDateTime arrived, out SqlString acktype, out SqlString ackmsg)
        {
            Message msg = (Message)obj;

            id = null;
            label = null;
            xmlmsg = null;
            arrived = SqlDateTime.Null;
            acktype = null;
            ackmsg = null;

            if (msg != null)
            {
                id = (msg.Id != null) ? (SqlString)msg.Id : null;
                label = (msg.Label != null) ? (SqlString)msg.Label : null;
                xmlmsg = (msg.BodyStream != null) ? new SqlXml(msg.BodyStream) : null;
                arrived = (SqlDateTime)msg.ArrivedTime;
                acktype = (SqlString)(msg.AcknowledgeType.ToString());
            }
        }

        /// <summary>
        /// Peeks at a specified message in queue
        /// </summary>
        /// <param name="queuepath">Queue path</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for the specified message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static bool PingQueue(string queuepath)
        {
            try
            {
                using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Peek))
                {
                    return queue.CanRead;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove all messages in a queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        [SqlFunction]
        public static int PurgeQueue(string queuepath)
        {
            int counter = 0;

            using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Receive))
            {
                queue.Refresh();
                queue.MessageReadPropertyFilter.ClearAll();

                MessageEnumerator msgEnum = queue.GetMessageEnumerator2();

                while (msgEnum.MoveNext(new TimeSpan(0, 0, 0)))
                {
                    counter++;
                    msgEnum.RemoveCurrent();
                    msgEnum.Reset();
                }
            }

            return counter;
        }

        /// <summary>
        /// Counts all snapshot messages in a specified queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        [SqlFunction]
        public static int CountInQueue(string queuepath)
        {
            int counter = 0;

            using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Peek))
            {
                queue.Refresh();
                queue.MessageReadPropertyFilter.ClearAll();

                MessageEnumerator msgEnum = queue.GetMessageEnumerator2();

                while (msgEnum.MoveNext(new TimeSpan(0, 0, 0)))
                {
                    counter++;
                }

                msgEnum.Close();
            }

            return counter;
        }

        /// <summary>
        /// Sends message to queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        /// <param name="msg">Message</param>
        [SqlFunction]
        public static string SendToQueue(string requestqueue, string responsequeue, string msgId, SqlXml xmlmsg)
        {
            string label = String.Empty;

            if (requestqueue.Length.Equals(0) || responsequeue.Length.Equals(0) || xmlmsg.Value.Length.Equals(0))
                return label;

            //Creates the byte array of the message to format the string properly
            byte[] byteMsg = SetEncoding(requestqueue, xmlmsg);

            //Sets the correlation and label of the queue message
            Guid guid = Guid.NewGuid();

            //Must include the slash to be in the correct format
            label = guid.ToString() + @"\" + msgId;

            Message msg = new Message();

            //ActiveXMessageFormatter adds the proper 0x00 format (Binary adds extra chars)
            msg.Formatter = new ActiveXMessageFormatter();

            //Set the Queue messages with the highest read priority
            msg.Priority = MessagePriority.Highest;
            msg.ResponseQueue = new MessageQueue(responsequeue);

            //CorrelationID is not sent back by Provenir, must use Label Id instead  - create PeekByLabel extension of MessageQueue
            //See MessageQueue class below
            msg.CorrelationId = label;

            //Use the returned label value to get the message on the outbound queues
            msg.Label = label;
            msg.BodyStream.Write(byteMsg, 0, byteMsg.Length);

            using (MessageQueue queue = new MessageQueue(requestqueue, QueueAccessMode.Send))
            {
                queue.Send(msg);
                return label;
            }
        }

        /// <summary>
        /// Sends message to queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        /// <param name="msg">Message</param>
        [SqlFunction]
        public static string SendWithAck(string requestqueue, string responsequeue, string ackqueue, string msgId, SqlXml xmlmsg)
        {
            string label = String.Empty;

            if (requestqueue.Length.Equals(0) || responsequeue.Length.Equals(0) || xmlmsg.Value.Length.Equals(0))
                return label;

            //Creates the byte array of the message to format the string properly
            byte[] byteMsg = SetEncoding(requestqueue, xmlmsg);

            //Sets the correlation and label of the queue message
            Guid guid = Guid.NewGuid();

            //Must include the slash to be in the correct format
            label = guid.ToString() + @"\" + msgId;

            Message msg = new Message();
            msg.AdministrationQueue = new MessageQueue(ackqueue);
            msg.AcknowledgeType = AcknowledgeTypes.FullReachQueue | AcknowledgeTypes.FullReceive;

            //ActiveXMessageFormatter adds the proper 0x00 format (Binary adds extra chars)
            msg.Formatter = new ActiveXMessageFormatter();

            //Set the Queue messages with the highest read priority
            msg.Priority = MessagePriority.Highest;
            msg.ResponseQueue = new MessageQueue(responsequeue);

            //CorrelationID is not sent back by Provenir, must use Label Id instead  - create PeekByLabel extension of MessageQueue
            //See MessageQueue class below
            msg.CorrelationId = label;

            //Use the returned label value to get the message on the outbound queues
            msg.Label = label;
            msg.BodyStream.Write(byteMsg, 0, byteMsg.Length);

            using (MessageQueue queue = new MessageQueue(requestqueue, QueueAccessMode.Send))
            {
                queue.Send(msg);

                return label;
            }
        }

        /// <summary>
        /// Peeks at the top message in a specified queue
        /// </summary>
        /// <param name="queuepath">Queue path</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for a message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable PeekFirst(string queuepath, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();

            //Do not allow the timeout to be more than the default Maximum
            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            try
            {
                using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Peek))
                {
                    queue.Refresh();
                    queue.MessageReadPropertyFilter.ClearAll();

                    queue.MessageReadPropertyFilter.Id = true;
                    queue.MessageReadPropertyFilter.Label = true;
                    queue.MessageReadPropertyFilter.ArrivedTime = true;
                    queue.MessageReadPropertyFilter.Body = true;
                    queue.MessageReadPropertyFilter.AdministrationQueue = true;
                    queue.MessageReadPropertyFilter.AcknowledgeType = true;
                    queue.MessageReadPropertyFilter.Acknowledgment = false;
                    queue.MessageReadPropertyFilter.CorrelationId = false;

                    //The message that was passed back from the Send request
                    msg = queue.Peek(TimeSpan.FromSeconds(timeoutseconds));

                    if (msg != null)
                    {
                        msgs.Add(msg);
                    }
                }
            }
            catch (MessageQueueException ex)
            {
                if (!ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.IOTimeout))
                    throw ex;
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// Gets a snapshot of messages in a specified queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable PeekInQueue(string queuepath)
        {
            using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Peek))
            {
                queue.Refresh();
                queue.MessageReadPropertyFilter.ClearAll();

                queue.MessageReadPropertyFilter.Id = true;
                queue.MessageReadPropertyFilter.Label = true;
                queue.MessageReadPropertyFilter.ArrivedTime = true;
                queue.MessageReadPropertyFilter.Body = true;
                queue.MessageReadPropertyFilter.AdministrationQueue = true;
                queue.MessageReadPropertyFilter.AcknowledgeType = true;
                queue.MessageReadPropertyFilter.Acknowledgment = false;
                queue.MessageReadPropertyFilter.CorrelationId = false;

                MessageEnumerator msgEnum = queue.GetMessageEnumerator2();
                List<Message> msgs = new List<Message>();

                while (msgEnum.MoveNext(new TimeSpan(0, 0, 0)))
                {
                    msgs.Add(msgEnum.Current);
                }

                msgEnum.Close();

                return msgs.ToArray();
            }
        }

        /// <summary>
        /// Peeks at a specified message in queue
        /// </summary>
        /// <param name="queuepath">Queue path</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for the specified message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable PeekById(string queuepath, string Id, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();

            if (Id.Length.Equals(0) || queuepath.Length.Equals(0))
                return msgs;

            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            try
            {
                using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Peek))
                {
                    queue.Refresh();
                    queue.MessageReadPropertyFilter.ClearAll();

                    queue.MessageReadPropertyFilter.Id = true;
                    queue.MessageReadPropertyFilter.Label = true;
                    queue.MessageReadPropertyFilter.ArrivedTime = true;
                    queue.MessageReadPropertyFilter.Body = true;
                    queue.MessageReadPropertyFilter.AdministrationQueue = true;
                    queue.MessageReadPropertyFilter.AcknowledgeType = true;
                    queue.MessageReadPropertyFilter.Acknowledgment = false;
                    queue.MessageReadPropertyFilter.CorrelationId = false;

                    //The message that was passed back from the Send request
                    msg = queue.PeekById(Id, TimeSpan.FromSeconds(timeoutseconds));

                    if (msg != null)
                    {
                        msgs.Add(msg);
                    }
                }
            }
            catch (MessageQueueException ex)
            {
                if (!ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.IOTimeout))
                    throw ex;
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// Peeks at a specified message in queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        /// <param name="msg">Message</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for a message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable PeekByLabel(string queuepath, string label, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();

            if (label.Length.Equals(0) || queuepath.Length.Equals(0))
                return msgs;

            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Receive))
            {
                //The message that was passed back from the Send request
                msg = MessageQueueHandler.PeekByLabel(queue, label, TimeSpan.FromSeconds(timeoutseconds));

                if (msg != null)
                {
                    msgs.Add(msg);
                }
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// Gets the top message in a specified queue
        /// </summary>
        /// <param name="queuepath">Queue path</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for a message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable ReceiveFirst(string queuepath, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();

            //Maximum Timeout is Six Minutes
            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            try
            {
                using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Receive))
                {
                    queue.Refresh();
                    queue.MessageReadPropertyFilter.ClearAll();

                    queue.MessageReadPropertyFilter.Id = true;
                    queue.MessageReadPropertyFilter.Label = true;
                    queue.MessageReadPropertyFilter.ArrivedTime = true;
                    queue.MessageReadPropertyFilter.Body = true;
                    queue.MessageReadPropertyFilter.AdministrationQueue = true;
                    queue.MessageReadPropertyFilter.AcknowledgeType = true;
                    queue.MessageReadPropertyFilter.Acknowledgment = false;
                    queue.MessageReadPropertyFilter.CorrelationId = false;

                    //The message that was passed back from the Send request
                    msg = queue.Receive(TimeSpan.FromSeconds(timeoutseconds));

                    if (msg != null)
                    {
                        msgs.Add(msg);

                        //Add Acknowledgements if they exist
                        if (msg.AdministrationQueue.Path.Length > 0)
                        {
                            Message ackmsg = msg.AdministrationQueue.ReceiveByCorrelationId(msg.Label);
                            msgs.Add(ackmsg);
                        }
                    }
                }
            }
            catch (MessageQueueException ex)
            {
                if (!ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.IOTimeout))
                    throw ex;
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// Receives a specified message in queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        /// <param name="msg">Message</param>
        /// <param name="timeoutseconds">How long in seconds the request looks for a message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable ReceiveById(string queuepath, string Id, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();


            if (Id.Length.Equals(0) || queuepath.Length.Equals(0))
                return msgs.ToArray();

            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            try
            {
                using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Receive))
                {
                    queue.Refresh();
                    queue.MessageReadPropertyFilter.ClearAll();

                    queue.MessageReadPropertyFilter.Id = true;
                    queue.MessageReadPropertyFilter.Label = true;
                    queue.MessageReadPropertyFilter.ArrivedTime = true;
                    queue.MessageReadPropertyFilter.Body = true;
                    queue.MessageReadPropertyFilter.AdministrationQueue = true;
                    queue.MessageReadPropertyFilter.AcknowledgeType = true;
                    queue.MessageReadPropertyFilter.Acknowledgment = false;
                    queue.MessageReadPropertyFilter.CorrelationId = false;

                    //The message that was passed back from the Send request
                    msg = queue.ReceiveById(Id, TimeSpan.FromSeconds(timeoutseconds));

                    if (msg != null)
                    {
                        msgs.Add(msg);
                    }
                }
            }
            catch (MessageQueueException ex)
            {
                if (!ex.MessageQueueErrorCode.Equals(MessageQueueErrorCode.IOTimeout))
                    throw ex;
            }

            return msgs.ToArray();
        }

        /// <summary>
        /// Receives message from queue
        /// </summary>
        /// <param name="queue">Queue path</param>
        /// <param name="msg">Message</param>
        [SqlFunction(FillRowMethodName = "InQueueFillRow")]
        public static IEnumerable ReceiveByLabel(string queuepath, string label, int timeoutseconds)
        {
            Message msg = null;
            List<Message> msgs = new List<Message>();

            if (label.Length.Equals(0) || queuepath.Length.Equals(0))
                return msgs;

            timeoutseconds = (timeoutseconds > MaxTimeoutSeconds) ? MaxTimeoutSeconds : timeoutseconds;

            using (MessageQueue queue = new MessageQueue(queuepath, QueueAccessMode.Receive))
            {
                //The message that was passed back from the Send request
                msg = MessageQueueHandler.ReceiveByLabel(queue, label, TimeSpan.FromSeconds(timeoutseconds));

                if (msg != null)
                {
                    msgs.Add(msg);
                }
            }

            return msgs.ToArray();
        }

        private static byte[] SetEncoding(string requestqueue, SqlXml xmlmsg)
        {
            byte[] byteMsg;

            string XmlString = string.Empty;

            XmlString = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>\n";
            XmlString += xmlmsg.Value;
            byteMsg = Encoding.Unicode.GetBytes(XmlString);

            return byteMsg;
        }
    }
}
